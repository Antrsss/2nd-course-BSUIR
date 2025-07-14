 org $8000

 ldaa #%11111101
 ldab #%10010011

 ldx #$f100
 staa 0,x
 stab 1,x
 staa 2,x
 stab 3,x

 ;старший байт регистра x

 bclr 0,x,#%00001111

 asr 1,x
 asr 1,x
 asr 1,x
 asr 1,x

 bclr 1,x,#%11110000

 clra
 adda 0,x
 adda 1,x

 ;младший байт регистра x

 asl 2,x
 asl 2,x
 asl 2,x
 asl 2,x

 bclr 3,x,#%11110000

 clrb
 addb 2,x
 addb 3,x

 ;записываем их в память и записываем в x

 staa $f100
 stab $f101

 ldx $f100

 