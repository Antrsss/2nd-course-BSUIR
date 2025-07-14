
data segment
    ans db 3  
    rans db 3 
    space db ' '
ends

stack segment
    dw   128  dup(0)
ends
  


code segment  
    
sin proc 
    mov bx, ax 
    
    mul ax
    mul ax 
    mov cx, 6
    div cx
    
    sub bx, ax 
    
    ret
endp   
    
start:
    mov ax, data 
    mov ds, ax
    
    mov ax, 1
    
    call sin 
    
    mov ans[0], bh
    add ans[0], '0'
    mov ans[1], bl 
    add ans[1], '0'
    
    mov ans[2], '$'
    
    mov rans[0], dh 
    add rans[0], '0'
    mov rans[1], dl
    add rans[1], '0'
    
    mov rans[2], '$'
    
    lea dx, ans
    mov ah, 9
    int 21h 
    
    mov dx, 9
    mov ah, 9
    int 21h 
    
    lea dx, rans
    mov ah, 9
    int 21h 
    
    mov ax, 4c00h
    int 21h    
ends

end start ; set entry point and stop the assembler.
