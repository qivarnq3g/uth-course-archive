$ErrorActionPreference = "Stop"
$base = "d:\courses\Network Programming\BTL\066206005044_BTL_CollaborativeWhiteboardLAN\066206005044_BTL_WhiteboardClient"
$mainFile = Join-Path $base "MainWindow.xaml.cs"
$rawLines = [System.IO.File]::ReadAllLines($mainFile, [System.Text.Encoding]::UTF8)
Write-Host "Read $($rawLines.Length) lines from $mainFile"

function Get-MethodRange {
    param([string[]]$lines, [string]$sig)
    $declLine = -1
    for ($i = 0; $i -lt $lines.Length; $i++) {
        if ($lines[$i] -match [regex]::Escape($sig)) {
            $declLine = $i; break
        }
    }
    if ($declLine -lt 0) {
        Write-Warning "NOT FOUND: $sig"
        return $null
    }
    $depth = 0; $started = $false; $endLine = -1
    for ($i = $declLine; $i -lt $lines.Length; $i++) {
        $line = $lines[$i]; $j = 0
        while ($j -lt $line.Length) {
            $ch = $line[$j]
            if ($ch -eq '{') { $depth++; $started = $true }
            elseif ($ch -eq '}') { $depth--; if ($started -and $depth -eq 0) { $endLine = $i; break } }
            $j++
        }
        if ($endLine -ge 0) { break }
    }
    if ($endLine -lt 0) { return $null }
    $start = $declLine
    if ($start -gt 0 -and $lines[$start - 1].Trim() -eq '') { $start-- }
    return @{ Start = $start; End = $endLine }
}

$groups = [ordered]@{
    "CanvasViewport" = @("private void CanvasViewportGrid_SizeChanged(", "private void ApplyCanvasViewportTransform()", "private void UpdateCanvasViewportClip()", "private void ClampCanvasPanToViewport()", "private void ResetCanvasViewport()", "private static double ClampCanvasZoom(", "private string GetCanvasZoomText()")
    "Keyboard" = @("private async void MainWindow_KeyDown(", "private void MainWindow_KeyUp(", "private void UpdateAltKeyState(", "private void UpdateSpaceKeyState(", "private bool IsAltPressedForZoom()", "private bool IsSpacePressedForPan()", "private static bool IsControlKeyDown()", "private bool TryHandlePanelTabShortcut(", "private bool IsTextInputFocused()")
    "StatusAndUI" = @("private async void UndoButton_Click(", "private async void RedoButton_Click(", "private async Task SendUndoAsync()", "private async Task SendRedoAsync()", "private bool CanApplyHistoryAction(", "private void SetStatus(", "private void SetConnectionState(", "private void UpdateConnectionControls(", "private void ApplyStatusBadge(", "private void UpdateOwnershipDisplay(", "private bool IsCurrentUserOwner()", "private void UpdateHostControlsVisibility()", "private void UpdateRoomLockVisuals()")
    "ConnectionFlow" = @("private async void RefreshRoomsButton_Click(", "private async Task RefreshRoomsAsync(", "private async void ConnectButton_Click(", "private async Task ConnectToSelectedRoomAsync()", "private async Task<bool> HandleFirstUserRaceAsync(", "private async Task DisconnectAsync()", "private void ResetOwnershipState()", "private async void SendChatButton_Click(", "private async void ChatInputTextBox_KeyDown(", "private async Task SendChatAsync()", "private string GetSafeUserName()", "private void UpdatePasswordInputGuidance()", "private static bool IsFirstJoinPasswordSetupMode(", "private void ApplyAuthoritativeRoomState(", "private async void SaveRoomPasswordButton_Click(", "private void RoomListView_SelectionChanged(", "private void RoomPasswordBox_PasswordChanged(")
    "Moderation" = @("private void UpdateBannedUsersVisuals()", "private void ApplyBanListSnapshot(", "private void UpdateOnlineUserListItemActions()", "private void OnlineUsersListView_SelectionChanged(", "private string? ResolveTargetUserIdFromSender(", "private async void KickUserButton_Click(", "private async void BanUserButton_Click(", "private async void HostUnbanUserButton_Click(", "private void BannedUsersComboBox_SelectionChanged(", "private async void HostClearCanvasButton_Click(", "private async void HostLockRoomButton_Click(", "private async void HostUnlockRoomButton_Click(")
    "ToolingAndColor" = @("private void StrokeColorComboBox_SelectionChanged(", "private void StrokeThicknessNumberBox_ValueChanged(", "private void CustomColorTextBox_KeyDown(", "private void ApplyCustomColorButton_Click(", "private void ColorSwatchButton_Click(", "private void AdvancedColorPicker_ColorChanged(", "private void AdvancedColorPickerFlyout_Closed(", "private void ToolToggleButton_Click(")
}

$headers = @{
    "CanvasViewport" = "using System;`nusing System.Globalization;`nusing Microsoft.UI.Xaml;`nusing Windows.Foundation;`n`nnamespace Btl.Whiteboard.Client;`n`npublic sealed partial class MainWindow`n{"
    "Keyboard" = "using System.Threading.Tasks;`nusing Microsoft.UI.Xaml;`nusing Microsoft.UI.Xaml.Controls;`nusing Microsoft.UI.Xaml.Input;`nusing Windows.System;`nusing Windows.UI.Core;`n`nnamespace Btl.Whiteboard.Client;`n`npublic sealed partial class MainWindow`n{"
    "StatusAndUI" = "using System;`nusing System.Diagnostics;`nusing System.Threading;`nusing System.Threading.Tasks;`nusing Microsoft.UI;`nusing Microsoft.UI.Xaml;`nusing Microsoft.UI.Xaml.Media;`n`nnamespace Btl.Whiteboard.Client;`n`npublic sealed partial class MainWindow`n{"
    "ConnectionFlow" = "using System;`nusing System.Collections.Generic;`nusing System.Linq;`nusing System.Threading;`nusing System.Threading.Tasks;`nusing Btl.Whiteboard.Client.Networking;`nusing Btl.Whiteboard.Contracts;`nusing Microsoft.UI.Xaml;`nusing Microsoft.UI.Xaml.Controls;`nusing Microsoft.UI.Xaml.Input;`nusing Windows.System;`n`nnamespace Btl.Whiteboard.Client;`n`npublic sealed partial class MainWindow`n{"
    "Moderation" = "using System;`nusing System.Collections.Generic;`nusing System.Linq;`nusing System.Threading.Tasks;`nusing Microsoft.UI.Xaml;`nusing Microsoft.UI.Xaml.Controls;`n`nnamespace Btl.Whiteboard.Client;`n`npublic sealed partial class MainWindow`n{"
}

$removeFlags = New-Object bool[] $rawLines.Length
$extracted = @{}
foreach ($group in $groups.Keys) {
    $sigs = $groups[$group]; $blocks = [System.Collections.Generic.List[string]]::new()
    foreach ($sig in $sigs) {
        $range = Get-MethodRange -lines $rawLines -sig $sig
        if ($null -eq $range) { continue }
        Write-Host "  [$group] $sig lines $($range.Start+1)-$($range.End+1)"
        for ($i = $range.Start; $i -le $range.End; $i++) { $removeFlags[$i] = $true; $blocks.Add($rawLines[$i]) }
        $blocks.Add("")
    }
    $extracted[$group] = $blocks
}

$newFiles = [ordered]@{"CanvasViewport"="MainWindow.CanvasViewport.cs"; "Keyboard"="MainWindow.Keyboard.cs"; "StatusAndUI"="MainWindow.StatusAndUI.cs"; "ConnectionFlow"="MainWindow.ConnectionFlow.cs"; "Moderation"="MainWindow.Moderation.cs"}
foreach ($group in $newFiles.Keys) {
    $outPath = Join-Path $base $newFiles[$group]
    $content = $headers[$group] + "`n" + ($extracted[$group] -join "`n") + "`n}"
    [System.IO.File]::WriteAllText($outPath, $content, [System.Text.Encoding]::UTF8)
    Write-Host "Created: $($newFiles[$group])"
}

$tcFile = Join-Path $base "MainWindow.ToolingAndColor.cs"
if (Test-Path $tcFile) {
    $tcLines = [System.IO.File]::ReadAllLines($tcFile, [System.Text.Encoding]::UTF8)
    $lastBrace = -1; for ($i=$tcLines.Length-1; $i -ge 0; $i--) { if ($tcLines[$i].Trim() -eq '}') { $lastBrace=$i; break } }
    if ($lastBrace -ge 0) {
        $newTc = [System.Collections.Generic.List[string]]::new(); for($i=0;$i -lt $lastBrace;$i++) { $newTc.Add($tcLines[$i]) }
        $newTc.Add(""); foreach($line in $extracted["ToolingAndColor"]) { $newTc.Add($line) }; $newTc.Add("}")
        [System.IO.File]::WriteAllLines($tcFile, $newTc, [System.Text.Encoding]::UTF8)
        Write-Host "Updated: MainWindow.ToolingAndColor.cs"
    }
}

$kept = [System.Collections.Generic.List[string]]::new(); $prevBlank = $false
for ($i=0; $i -lt $rawLines.Length; $i++) {
    if ($removeFlags[$i]) { continue }
    $isBlank = ($rawLines[$i].Trim() -eq '')
    if ($isBlank -and $prevBlank) { continue }
    $kept.Add($rawLines[$i]); $prevBlank = $isBlank
}
[System.IO.File]::WriteAllLines($mainFile, $kept, [System.Text.Encoding]::UTF8)
Write-Host "Rewrote MainWindow.xaml.cs: $($kept.Count) lines (was $($rawLines.Length))"
Write-Host "Done."
