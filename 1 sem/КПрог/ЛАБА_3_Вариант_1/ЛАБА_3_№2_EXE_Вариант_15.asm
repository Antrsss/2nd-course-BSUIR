data segment
    write_string db "Write string (max character's number = 200): $"
    write_end_string db "New string: "   
    new_line db 0x0A, 0x0D, '$'
    input_string db 201
                 db 201 dup(?)
ends

stack segment
    db 200 dup(0)
ends  
    
    
display_str macro my_str, char_count
    mov ah, 09h
    lea dx, [my_str]
    mov cx, char_count
    int 21h
endm

write_word macro
    mov [si], 'n'
    inc si
    mov [si], 'u'
    inc si
    mov [si], 'm'
    inc si
    mov [si], 'b'
    inc si
    mov [si], 'e'
    inc si
    mov [si], 'r' 
    inc si
    mov [si], ' ' 
    add bp, 7
endm 
  
  
write_string_to_stack macro out_str,   
    lea si, input_string + 2 ;first string character 
    mov al, [si - 1] 
    mov ah, 0
    lea di, input_string + 1
    add di, ax      ;last string character
    mov ax, si
    mov si, di
    inc si
    mov bh, 0
    mov cx, 0
    
to_stack:
    cmp si, ax                
    jb after
    
    mov bl, [si]
    
    push bx
    inc cx
    dec si
    
    cmp bl, ' '
    je add_star
    cmp bl, 0x0D
    je add_star
    
    jmp to_stack 

add_star:
    mov bl, '*'
    push bx  
    inc cx
    
    jmp to_stack
    
after:
    inc si ;first string character
endm 


code segment
start: 

    mov ax, stack
    mov ss, ax
    mov sp, 128
    
    mov ax, data
    mov ds, ax

    display_str write_string, 45
        
    lea dx, input_string 
    mov ah, 0Ah     
    int 21h  
    
    write_string_to_stack input_string   
    ; si - fist string character
    ; ' ', '*' <-- stack  
    mov dx, 0
    mov bx, 1
    mov bp, cx ;count of chars in end string
    
from_stack_to_string: 
    cmp cx, 0
    je show_string
    
    pop ax
    dec cx
    
    cmp al, '*'
    je check_if_number 
    
    mov [si], al
    inc si
    inc dx
    
    cmp al, ' '
    je new_word 
    
    cmp al, '0'               
    jb not_a_number         
    cmp al, '9'               
    ja not_a_number 
    
    jmp from_stack_to_string  
    
new_word:
    mov bx, 1 ;is_a_number 
    mov dx, 0
    jmp from_stack_to_string
    
not_a_number: 
    mov bx, 0
    jmp from_stack_to_string   

check_if_number: 
    dec bp
    
    cmp bx, 0
    je from_stack_to_string 
    
    mov ax, 0
    mov bx, dx
    dec si
    
write_word_to_stack:
    cmp dx, 0
    je rewrite_string
    
    mov al, [si]
    
    push ax
    inc cx 
    
    dec si
    dec dx 
    jmp write_word_to_stack
    
rewrite_string:
    inc si 
    write_word
    inc si
    
    cmp dx, 0
    je from_stack_to_string
      
show_string: 
    mov [si], '$'       
    display_str new_line, 2 
    display_str write_end_string, 12
    display_str input_string+2, bp

end_program:
    mov ax, 4c00h ; exit to operating system.
    int 21h    
ends

end start ; set entry point and stop the assembler. 