 org $8000
 
;������� + �����
 ldx #$8200
 ldab #10
 stab 1,x
 ldab #11
 stab 2,x
 ldab #12
 stab 5,x

;���������� �
 ldaa #3
 staa $fff1
 ldab #0
 stab $fff0

;������ + ����� � ����
loop:
 ldab 0,x
 inx
 tstb 
 bmi loop ;���� -
 beq loop ;���� 0
 pshb
 ldab #0
 pshb
 deca
 cmpa #0
 bne loop ;  �� =

;�����
 ldy $fff0
 ldd #0

;������������
loop2:
 pulx
 stx $ff00
 addd $ff00
 dey
 
 cmpy #0
 bne loop2
 
;�����
 xgdy
 

 

 

 