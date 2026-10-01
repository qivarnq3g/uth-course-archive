name "calc-sum"
org 100h

xor ax, ax       
mov cl, n
xor ch, ch      
mov bl, 1         

vonglap: add ax, bx
        cmp bl, cl
        je end    
        inc bl
        jmp vonglap

end: mov [sum], ax
    ret

n db 100
sum dw ?
