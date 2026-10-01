org 100h

jmp main

msg_info    db 'Hoang Viet Quang - 066206005044', 13, 10
            db 'Nhap mot so tu 0 den 9: $'
            
msg_lon     db 13, 10, 'So nhap vao lon hon so 4.$'
msg_nho     db 13, 10, 'So nhap vao nho hon so 4.$'
msg_bang    db 13, 10, 'So nhap vao bang so 4.$'
msg_nhiphan db 13, 10, 'Gia tri nhi phan: $'

input_val   db ?


main:
    mov dx, offset msg_info
    mov ah, 9
    int 21h

    mov ah, 1
    int 21h

    sub al, 30h
    mov [input_val], al

    cmp al, 4
    jna nho_hon_hoac_bang
    
    mov dx, offset msg_lon
    mov ah, 9
    int 21h
    jmp in_nhi_phan
    
nho_hon_hoac_bang:
    cmp al, 4
    je bang_nhau
    
    mov dx, offset msg_nho
    mov ah, 9
    int 21h
    jmp in_nhi_phan
    
bang_nhau:
    mov dx, offset msg_bang
    mov ah, 9
    int 21h
    
in_nhi_phan:
    mov dx, offset msg_nhiphan
    mov ah, 9
    int 21h
    
    mov bl, [input_val]
    mov cx, 8
    
vong_lap_bit:
    shl bl, 1
    
    mov dl, '0'
    jnc in_bit
    mov dl, '1'
    
in_bit:
    mov ah, 2
    int 21h
    
    loop vong_lap_bit

ret