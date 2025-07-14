 org $8000
 
;задание + чисел
 ldx #$8200
 ldab #10
 stab 1,x
 ldab #11
 stab 2,x
 ldab #12
 stab 5,x

;сохранение А
 ldaa #3
 staa $fff1
 ldab #0
 stab $fff0

;запись + чисел в стек
loop:
 ldab 0,x
 inx
 tstb 
 bmi loop ;байт -
 beq loop ;байт 0
 pshb
 ldab #0
 pshb
 deca
 cmpa #0
 bne loop ;  не =

;после
 ldy $fff0
 ldd #0

;суммирование
loop2:
 pulx
 stx $ff00
 addd $ff00
 dey
 
 cmpy #0
 bne loop2
 
;после
 xgdy
 

 

 

 