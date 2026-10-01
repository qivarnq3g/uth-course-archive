name "flags"
org 100h

ex1:
    mov dx, offset msg1   
    mov ah, 9
    int 21h

    mov ah, 1              
    int 21h
    mov bl, al            

    mov dl, 0Dh
    mov ah, 2
    int 21h
    mov dl, 0Ah
    int 21h

    cmp bl, '0'
    jb not_digit          
    cmp bl, '9'
    ja not_digit           

    jmp stop               

not_digit:
    mov dx, offset msg2   
    mov ah, 9
    int 21h
    jmp ex1               

stop:
    ret                    

msg1 db "Nhap vao mot ky tu: ", "$"
msg2 db "Ban da nhap dung", 0Dh, 0Ah, "$"
