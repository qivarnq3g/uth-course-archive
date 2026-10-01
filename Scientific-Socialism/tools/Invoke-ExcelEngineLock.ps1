[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('Acquire', 'Release', 'Status')]
    [string]$Action,

    [Parameter(Mandatory = $true)]
    [string]$Workbook,

    [ValidateSet('excel-cli', 'minimax-xlsx')]
    [string]$Engine,

    [string]$Token,

    [switch]$RecoverStale
)

$ErrorActionPreference = 'Stop'

function Get-WorkbookLockInfo {
    param([string]$WorkbookPath)

    $absolutePath = [System.IO.Path]::GetFullPath($WorkbookPath)
    $normalizedPath = $absolutePath.ToLowerInvariant()
    $sha256 = [System.Security.Cryptography.SHA256]::Create()
    try {
        $hashBytes = $sha256.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($normalizedPath))
    }
    finally {
        $sha256.Dispose()
    }

    $hash = [System.BitConverter]::ToString($hashBytes).Replace('-', '').ToLowerInvariant()
    $lockRoot = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) 'Codex\excel-workbook-locks'
    [pscustomobject]@{
        Workbook = $absolutePath
        LockRoot = $lockRoot
        LockPath = Join-Path $lockRoot "$hash.json"
    }
}

function Read-LockFile {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        return $null
    }
    try {
        return Get-Content -Raw -LiteralPath $Path | ConvertFrom-Json
    }
    catch {
        throw "Lock file is unreadable and was left untouched: $Path"
    }
}

function Test-OwnerAlive {
    param($LockData)
    if ($null -eq $LockData -or $null -eq $LockData.pid) {
        return $false
    }
    return $null -ne (Get-Process -Id ([int]$LockData.pid) -ErrorAction SilentlyContinue)
}

$info = Get-WorkbookLockInfo -WorkbookPath $Workbook

if ($Action -eq 'Status') {
    $existing = Read-LockFile -Path $info.LockPath
    if ($null -eq $existing) {
        [pscustomobject]@{ status = 'unlocked'; workbook = $info.Workbook; lockPath = $info.LockPath } |
            ConvertTo-Json -Compress
        exit 0
    }
    $status = if (Test-OwnerAlive -LockData $existing) { 'locked' } else { 'stale' }
    [pscustomobject]@{ status = $status; workbook = $info.Workbook; lockPath = $info.LockPath; owner = $existing } |
        ConvertTo-Json -Depth 5 -Compress
    exit 0
}

if ($Action -eq 'Acquire') {
    if ([string]::IsNullOrWhiteSpace($Engine)) {
        throw '-Engine is required for Acquire.'
    }
    New-Item -ItemType Directory -Force -Path $info.LockRoot | Out-Null

    for ($attempt = 0; $attempt -lt 2; $attempt++) {
        $existing = Read-LockFile -Path $info.LockPath
        if ($null -ne $existing) {
            $ownerAlive = Test-OwnerAlive -LockData $existing
            if (-not $RecoverStale -or $ownerAlive) {
                $ownerState = if ($ownerAlive) { 'active' } else { 'stale or from a completed shell' }
                throw "Workbook is already locked by engine '$($existing.engine)' (PID $($existing.pid), $ownerState): $($info.Workbook). Release it with its token; use -RecoverStale only after verifying the owner is gone."
            }
            Remove-Item -LiteralPath $info.LockPath -Force
        }

        $newToken = [guid]::NewGuid().ToString('N')
        $payload = [ordered]@{
            version = 1
            workbook = $info.Workbook
            engine = $Engine
            pid = $PID
            acquiredAtUtc = [DateTime]::UtcNow.ToString('o')
            token = $newToken
        }
        $json = $payload | ConvertTo-Json -Compress

        try {
            $stream = [System.IO.File]::Open($info.LockPath, [System.IO.FileMode]::CreateNew,
                [System.IO.FileAccess]::Write, [System.IO.FileShare]::None)
            try {
                $bytes = [System.Text.Encoding]::UTF8.GetBytes($json)
                $stream.Write($bytes, 0, $bytes.Length)
            }
            finally {
                $stream.Dispose()
            }
            [pscustomobject]@{
                status = 'acquired'
                workbook = $info.Workbook
                engine = $Engine
                token = $newToken
                lockPath = $info.LockPath
            } | ConvertTo-Json -Compress
            exit 0
        }
        catch [System.IO.IOException] {
            if ($attempt -eq 1) { throw }
        }
    }
}

if ($Action -eq 'Release') {
    if ([string]::IsNullOrWhiteSpace($Token)) {
        throw '-Token is required for Release.'
    }
    $existing = Read-LockFile -Path $info.LockPath
    if ($null -eq $existing) {
        throw "Workbook is not locked: $($info.Workbook)"
    }
    if ($existing.token -ne $Token) {
        throw "Token does not own this workbook lock; lock was left untouched: $($info.Workbook)"
    }
    Remove-Item -LiteralPath $info.LockPath -Force
    [pscustomobject]@{ status = 'released'; workbook = $info.Workbook; engine = $existing.engine } |
        ConvertTo-Json -Compress
}
