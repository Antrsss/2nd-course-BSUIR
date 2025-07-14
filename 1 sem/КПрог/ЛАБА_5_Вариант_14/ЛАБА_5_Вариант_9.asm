data segment
    registers db 3 dup (?)
    buffer_count db 2 dup (?)  
    word_to_change db 11,0, 10 dup (?) 
    new_word db 11, 0, 10 dup (?)
    mess_word_to_change db 'Word to change: ', '$'  
    mess_new_word db 0Ah, 0Dh, 'New word: ', '$'  
    
    file_name db 'input.txt', 0    
    mess_error_open_file db 0Ah, 0Dh, 'File error!','$' 
    
    mess_data_file db 0Ah, 0Dh, 'Data of file: ', '$'
    mess_new_data_file db 0Ah, 0Dh, 'New data of file: ', '$'  
    
    no_arg_msg db 0Ah, 0Dh, 'Error: No word provided as command-line argument.', '$'  
      
    new_line db 0x0A, 0x0D, '$' 
    buffer db 1024 dup(?)  
ends

stack segment
    dw   128  dup(0)
ends
     
print_str macro out_str
    mov dx, offset out_str  
    mov ah, 9
    int 21h
endm 


;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
code segment
    mov ax, data
    mov ds, ax
    mov es, ax  
    
    print_str mess_word_to_change   
    lea dx, word_to_change
    mov ah, 0Ah 
    int 21h
    
    print_str mess_new_word   
    lea dx, new_word
    mov ah, 0Ah 
    int 21h 
     
    ;open file
    print_str mess_data_file  
    mov dx, offset file_name 
    mov ah, 3Dh 
    mov al, 00h ;open for reading 
    int 21h  
    
    jc error_open_file 
    mov bx, ax 
    mov di, 01  
    
read_data_in_file: 
    mov cx, 1024 
    mov dx, offset buffer 
    mov ah, 3Fh 
    int 21h   
    jc error_open_file  
  
    mov cx, ax 
    jcxz write_buffer_to_stack 
    
    mov ah, 40h 
    xchg bx, di 
    int 21h 
    
    xchg di, bx 
    jc close_file 
    jmp read_data_in_file  
              
write_buffer_to_stack:
    mov bx, 0 
    mov bl, '$'
    push bx
    mov bx, 0 
    lea si, buffer  
    
buffer_counting:
    cmp [si], 0 
    je count_end
    inc bx
    inc si
    jmp buffer_counting 
    
count_end:
    mov [buffer_count], bh
    mov [buffer_count+1], bl
    mov ah, 0 
    mov cx, 0
    
to_stack:
    mov al, [buffer+bx-1] 
    cmp al, '$'
    je end_writing
    
    mov cl, [word_to_change+1] ;char count
    mov di, offset word_to_change ;adress
    add di, cx ;adress of last character
    inc di
    mov si, 1 ;is or not equal
    mov bp, bx 
    
next_check: 
    cmp al, [di]                   
    jne set_not_equal              
   
    dec cx
    cmp cx, 0
    je check_ended  
    
    dec di
    dec bx
    mov al, [buffer+bx-1]                    
    jmp next_check                 

set_not_equal:
    mov si, 0  
    
check_ended: 
    cmp si, 1
    je write_new_word
    
    mov bx, bp
    mov al, [buffer+bx-1]
    push ax 
    dec bx 
    jmp to_stack
    
write_new_word:
    mov word ptr [registers], ax
    mov word ptr [registers+2], cx
    mov word ptr [registers+4], si
    
    mov ax, 0 
    mov cx, 0
    mov cl, [new_word+1] 
    mov si, offset new_word
    add si, cx 
    inc si
    
write_char:
    mov al, [si]
    push ax 
    
    dec si
    dec cx 
    cmp cx, 0
    jne write_char
    
    sub bx, 1
    mov ax, word ptr [registers]     
    mov cx, word ptr [registers+2]   
    mov si, word ptr [registers+4]    
    jmp to_stack         
          
end_writing: 
    mov cx, 0
    mov si, offset buffer
    mov ax, 0 
    
clear_buffer:
    cmp [si], 0
    je end_clearing 
    
    mov [si], 0
    inc si
    jmp clear_buffer

end_clearing:
    mov si, offset buffer    
    
write_to_buffer:
    pop ax
    cmp al, '$'
    je write_in_file
    
    mov [si], al
    inc si
    inc cx
    jmp write_to_buffer        
              
error_open_file:
    print_str mess_error_open_file
    jmp end_program  
    
write_in_file:
    mov dx, offset file_name
    mov ah, 3Ch
    mov al, 0
    int 21h 
    jc error_open_file  
    
    mov bx, ax
    lea dx, buffer
    mov cx, 1024
    mov ah, 40h
    int 21h
    jc error_open_file 
    
    mov ah, 3Eh
    int 21h
    jmp close_file 
    
close_file:
    mov ah,3Eh
    int 21h
    jmp end_program  
    
end_program:
    mov [si], '$'
    print_str new_line
    print_str mess_new_data_file 
    print_str buffer
    mov ax, 4c00h
    int 21h    
ends