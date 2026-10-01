; The easiest way to print out "Hello, World!"

name "hi"

org 100h

jmp start       ; jump over data declaration

msg:    db      "Name: Hoang Viet Quang",0Ah, 0dh, 24h 
ID:     db      "ID: 066206005044", 0Ah, 0Dh, 24h

start:  mov     dx, msg  ; load offset of msg into dx.
        mov     ah, 09h  ; print function is 9.
        int     21h      ; do it!  
        
        mov     dx, ID  ; load offset of msg into dx.
        mov     ah, 09h  ; print function is 9.
        int     21h      
        
        mov     ah, 0 
        int     16h      ; wait for any key....
        
ret ; return to operating system.