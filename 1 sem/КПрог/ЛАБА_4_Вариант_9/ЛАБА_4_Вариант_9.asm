data segment
    start_str db 'Input 30 numbers: $'  
    overflow_msg db 'Error: Overflow occurred!$' 
    quotient db 'Quotient: $'
    rest db ' Rest: $'
    answer db 6 dup ('0'), '$'
    arr db 30 dup (?)
data ends

code segment
    assume cs:code, ds:data


;input proc
input_arr proc
    lea dx, start_str 
    mov ah, 9
    int 21h 

    mov cx, 30      
    mov si, 0  
      
    mov dl, 0Dh    
    mov ah, 2
    int 21h 
    mov dl, 0Ah    
    mov ah, 2
    int 21h       

input:
input_number:
    mov ah, 1        
    int 21h     
    mov arr[si], al  
    inc si 
    
    sub al, '0' 
    cmp al, 221 
    jne input_number             

    mov dl, 0Ah    
    mov ah, 2
    int 21h

    loop input
    
    mov arr[si], 0Dh   

    ret
input_arr endp 
     

;find max and min proc     
find_max_min proc   
    pop bp 
    pop ax
    mov bx, ax
    mov si, 29
    
find:
    cmp si, 0 
    je done
    
    dec si
    pop cx
    cmp cx, ax 
    jle bx_check
    mov ax, cx  
    
bx_check:
    cmp cx, bx
    jge find 
    mov bx, cx
    jmp find 

done:
    push bp
    ret  
find_max_min endp  


;convert_to_str proc
convert_ax_to_string proc
    mov si, 0
    mov bp, 0                 
    mov bx, 10               

convert_loop_ax:
    inc bp
    mov dx, 0              
    div bx                  
    add dx, '0'             
    push dx  
    cmp ax, 0               
    jne convert_loop_ax                       
    
from_stack:
    dec bp
    pop cx
    mov answer[si], ch
    mov answer[si+1], cl
    add si, 2 
    cmp bp, 0
    je from_stack_end
    
from_stack_end:
    mov answer[si], '$'
    ret    
    
convert_ax_to_string endp
  

;print_str proc
print_str macro str   
    lea dx, str
    mov ah, 9
    int 21h 
endm
  

;program 
start:
    mov ax, data  
    mov ds, ax
    call input_arr  
    
    mov si, 0
    mov ax, 0
    mov bx, 10 
    mov ch, 0 
               
convert_to_number:
    mov cl, arr[si]  
    sub cl, '0' 
    cmp cl, 221               
    je last_step             

    mul bx
    jc overflow               
    add ax, cx  

    inc si             
    jmp convert_to_number

last_step:
    inc si 
    mov cl, arr[si]   
    cmp cl, 0Dh
    
    push ax
    mov ax, 0 
    
    je complete
    jmp convert_to_number 
         
overflow:
    mov ah, 09h
    lea dx, overflow_msg
    int 21h
    mov ax, 4C01h       
    int 21h
    
complete:  
    call find_max_min  
    
    mov cx, ax
    
    cmp ax, bx
    jl first
    sub ax, bx
    jmp division

first:
    sub bx, ax
    mov ax, bx
    
division:  
    div cx 
    
    mov arr[0], dh
    mov arr[1], dl
    call convert_ax_to_string
    print_str quotient 
    print_str answer
    
    mov ah, arr[0]
    mov al, arr[1] 
    call convert_ax_to_string
    print_str rest
    print_str answer
     
    mov ax, 4C01h       
    int 21h    
end start
code ends