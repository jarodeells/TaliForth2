\ From: John Hayes S1I
\ Subject: core.fr
\ Date: Mon, 27 Nov 95 13:10

\ Modified by SamCo 2018-05 for testing Tali Forth 2.
\ The main change is lowercasing all of the words as Tali
\ is case sensitive.

\ (C) 1995 JOHNS HOPKINS UNIVERSITY / APPLIED PHYSICS LABORATORY
\ MAY BE DISTRIBUTED FREELY AS LONG AS THIS COPYRIGHT NOTICE REMAINS.
\ VERSION 1.2
\ THIS PROGRAM TESTS THE CORE WORDS OF AN ANS FORTH SYSTEM.
\ THE PROGRAM ASSUMES A TWO'S COMPLEMENT IMPLEMENTATION WHERE
\ THE RANGE OF SIGNED NUMBERS IS -2^(N-1) ... 2^(N-1)-1 AND
\ THE RANGE OF UNSIGNED NUMBERS IS 0 ... 2^(N)-1.
\ I HAVEN'T FIGURED OUT HOW TO TEST KEY, QUIT, ABORT, OR ABORT"...

testing core words
hex

marker core_tests

\ ------------------------------------------------------------------------
testing basic assumptions

T{ -> }T     \ Start with clean slate
( test if any bits are set; answer in base 1 )
T{ : bitsset? if 0 0 else 0 then ; -> }T
T{  0 bitsset? -> 0 }T   \ zero is all bits clear
T{  1 bitsset? -> 0 0 }T \ other number have at least one bit
T{ -1 bitsset? -> 0 0 }T

\ ------------------------------------------------------------------------
testing booleans: and invert or xor

T{ 0 0 and -> 0 }T
T{ 0 1 and -> 0 }T
T{ 1 0 and -> 0 }T
T{ 1 1 and -> 1 }T

T{ 0 invert 1 and -> 1 }T
T{ 1 invert 1 and -> 0 }T

0 constant 0s
0 invert constant 1s

T{ 0s invert -> 1s }T
T{ 1s invert -> 0s }T

T{ 0s 0s and -> 0s }T
T{ 0s 1s and -> 0s }T
T{ 1s 0s and -> 0s }T
T{ 1s 1s and -> 1s }T

T{ 0s 0s or -> 0s }T
T{ 0s 1s or -> 1s }T
T{ 1s 0s or -> 1s }T
T{ 1s 1s or -> 1s }T

T{ 0s 0s xor -> 0s }T
T{ 0s 1s xor -> 1s }T
T{ 1s 0s xor -> 1s }T
T{ 1s 1s xor -> 0s }T

\ ------------------------------------------------------------------------
testing 2* 2/ lshift rshift

( we trust 1s, invert, and bitsset?; we will confirm rshift later )
1s 1 rshift invert constant msb
T{ msb bitsset? -> 0 0 }T

T{ 0s 2* -> 0s }T
T{ 1 2* -> 2 }T
T{ 4000 2* -> 8000 }T
T{ 1s 2* 1 xor -> 1s }T
T{ msb 2* -> 0s }T

T{ 0s 2/ -> 0s }T
T{ 1 2/ -> 0 }T
T{ 4000 2/ -> 2000 }T
T{ 1s 2/ -> 1s }T \ msb propogated
T{ 1s 1 xor 2/ -> 1s }T
T{ msb 2/ msb and -> msb }T

T{ 1 0 lshift -> 1 }T
T{ 1 1 lshift -> 2 }T
T{ 1 2 lshift -> 4 }T
T{ 1 f lshift -> 8000 }T \ biggest guaranteed shift
T{ 1s 1 lshift 1 xor -> 1s }T
T{ msb 1 lshift -> 0 }T

T{ 1 0 rshift -> 1 }T
T{ 1 1 rshift -> 0 }T
T{ 2 1 rshift -> 1 }T
T{ 4 2 rshift -> 1 }T
T{ 8000 f rshift -> 1 }T \ biggest
T{ msb 1 rshift msb and -> 0 }T  \ rshift zero fills msbs
T{ msb 1 rshift 2* -> msb }T

\ ------------------------------------------------------------------------
testing comparisons: true false 0= 0<> = <> 0< 0> < > u< min max within
0 invert  constant max-uint
0 invert 1 rshift  constant max-int
0 invert 1 rshift invert  constant min-int
0 invert 1 rshift  constant mid-uint
0 invert 1 rshift invert  constant mid-uint+1

0s constant <false>
1s constant <true>

T{ false -> 0 }T
T{ false -> <false> }T

T{ true -> <true> }T
T{ true -> 0 invert }T

T{ 0 0= -> <true> }T
T{ 1 0= -> <false> }T
T{ 2 0= -> <false> }T
T{ -1 0= -> <false> }T
T{ max-uint 0= -> <false> }T
T{ min-int 0= -> <false> }T
T{ max-int 0= -> <false> }T

T{ 0 0<> -> <false> }T
T{ 1 0<> -> <true> }T
T{ 2 0<> -> <true> }T
T{ -1 0<> -> <true> }T
T{ max-uint 0<> -> <true> }T
T{ min-int 0<> -> <true> }T
T{ max-int 0<> -> <true> }T

T{ 0 0 = -> <true> }T
T{ 1 1 = -> <true> }T
T{ -1 -1 = -> <true> }T
T{ 1 0 = -> <false> }T
T{ -1 0 = -> <false> }T
T{ 0 1 = -> <false> }T
T{ 0 -1 = -> <false> }T

T{ 0 0 <> -> <false> }T
T{ 1 1 <> -> <false> }T
T{ -1 -1 <> -> <false> }T
T{ 1 0 <> -> <true> }T
T{ -1 0 <> -> <true> }T
T{ 0 1 <> -> <true> }T
T{ 0 -1 <> -> <true> }T

T{ 0 0< -> <false> }T
T{ -1 0< -> <true> }T
T{ min-int 0< -> <true> }T
T{ 1 0< -> <false> }T
T{ max-int 0< -> <false> }T

T{ 0 0> -> <false> }T
T{ -1 0> -> <false> }T
T{ min-int 0> -> <false> }T
T{ 1 0> -> <true> }T
T{ max-int 0> -> <true> }T

T{ 0 1 < -> <true> }T
T{ 1 2 < -> <true> }T
T{ -1 0 < -> <true> }T
T{ -1 1 < -> <true> }T
T{ min-int 0 < -> <true> }T
T{ min-int max-int < -> <true> }T
T{ 0 max-int < -> <true> }T
T{ 0 0 < -> <false> }T
T{ 1 1 < -> <false> }T
T{ 1 0 < -> <false> }T
T{ 2 1 < -> <false> }T
T{ 0 -1 < -> <false> }T
T{ 1 -1 < -> <false> }T
T{ 0 min-int < -> <false> }T
T{ max-int min-int < -> <false> }T
T{ max-int 0 < -> <false> }T

T{ 0 1 > -> <false> }T
T{ 1 2 > -> <false> }T
T{ -1 0 > -> <false> }T
T{ -1 1 > -> <false> }T
T{ min-int 0 > -> <false> }T
T{ min-int max-int > -> <false> }T
T{ 0 max-int > -> <false> }T
T{ 0 0 > -> <false> }T
T{ 1 1 > -> <false> }T
T{ 1 0 > -> <true> }T
T{ 2 1 > -> <true> }T
T{ 0 -1 > -> <true> }T
T{ 1 -1 > -> <true> }T
T{ 0 min-int > -> <true> }T
T{ max-int min-int > -> <true> }T
T{ max-int 0 > -> <true> }T

T{ 0 1 u< -> <true> }T
T{ 1 2 u< -> <true> }T
T{ 0 mid-uint u< -> <true> }T
T{ 0 max-uint u< -> <true> }T
T{ mid-uint max-uint u< -> <true> }T
T{ 0 0 u< -> <false> }T
T{ 1 1 u< -> <false> }T
T{ 1 0 u< -> <false> }T
T{ 2 1 u< -> <false> }T
T{ mid-uint 0 u< -> <false> }T
T{ max-uint 0 u< -> <false> }T
T{ max-uint mid-uint u< -> <false> }T

T{ 1 0 u> -> <true> }T
T{ 2 1 u> -> <true> }T
T{ mid-uint 0 u> -> <true> }T
T{ max-uint 0 u> -> <true> }T
T{ max-uint mid-uint u> -> <true> }T
T{ 0 0 u> -> <false> }T
T{ 1 1 u> -> <false> }T
T{ 0 1 u> -> <false> }T
T{ 1 2 u> -> <false> }T
T{ 0 mid-uint u> -> <false> }T
T{ 0 max-uint u> -> <false> }T
T{ mid-uint max-uint u> -> <false> }T

T{ 0 1 min -> 0 }T
T{ 1 2 min -> 1 }T
T{ -1 0 min -> -1 }T
T{ -1 1 min -> -1 }T
T{ min-int 0 min -> min-int }T
T{ min-int max-int min -> min-int }T
T{ 0 max-int min -> 0 }T
T{ 0 0 min -> 0 }T
T{ 1 1 min -> 1 }T
T{ 1 0 min -> 0 }T
T{ 2 1 min -> 1 }T
T{ 0 -1 min -> -1 }T
T{ 1 -1 min -> -1 }T
T{ 0 min-int min -> min-int }T
T{ max-int min-int min -> min-int }T
T{ max-int 0 min -> 0 }T

T{ 0 1 max -> 1 }T
T{ 1 2 max -> 2 }T
T{ -1 0 max -> 0 }T
T{ -1 1 max -> 1 }T
T{ min-int 0 max -> 0 }T
T{ min-int max-int max -> max-int }T
T{ 0 max-int max -> max-int }T
T{ 0 0 max -> 0 }T
T{ 1 1 max -> 1 }T
T{ 1 0 max -> 1 }T
T{ 2 1 max -> 2 }T
T{ 0 -1 max -> 0 }T
T{ 1 -1 max -> 1 }T
T{ 0 min-int max -> 0 }T
T{ max-int min-int max -> max-int }T
T{ max-int 0 max -> max-int }T

T{ 1 2 4 within -> <false> }T
T{ 2 2 4 within -> <true> }T
T{ 3 2 4 within -> <true> }T
T{ 4 2 4 within -> <false> }T
T{ 5 2 4 within -> <false> }T

T{ 0 2 4 within -> <false> }T
T{ 1 0 4 within -> <true> }T
T{ 0 0 4 within -> <true> }T
T{ 4 0 4 within -> <false> }T
T{ 5 0 4 within -> <false> }T

T{ -1 -3 -1 within -> <false> }T
T{ -2 -3 -1 within -> <true> }T
T{ -3 -3 -1 within -> <true> }T
T{ -4 -3 -1 within -> <false> }T

T{ -2 -2 0 within -> <true> }T
T{ -1 -2 0 within -> <true> }T
T{ 0 -2 0 within -> <false> }T
T{ 1 -2 0 within -> <false> }T

T{ 0 min-int max-int within -> <true> }T
T{ 1 min-int max-int within -> <true> }T
T{ -1 min-int max-int within -> <true> }T
T{ min-int min-int max-int within -> <true> }T
T{ max-int min-int max-int within -> <false> }T

\ ------------------------------------------------------------------------
testing stack ops: 2drop 2dup 2over 2swap ?dup depth drop dup nip over rot -rot 
testing stack ops: swap tuck pick

T{ 1 2 2drop -> }T
T{ 1 2 2dup -> 1 2 1 2 }T
T{ 1 2 3 4 2over -> 1 2 3 4 1 2 }T
T{ 1 2 3 4 2swap -> 3 4 1 2 }T
T{ 0 ?dup -> 0 }T
T{ 1 ?dup -> 1 1 }T
T{ -1 ?dup -> -1 -1 }T
T{ depth -> 0 }T
T{ 0 depth -> 0 1 }T
T{ 0 1 depth -> 0 1 2 }T
T{ 0 drop -> }T
T{ 1 2 drop -> 1 }T
T{ 1 dup -> 1 1 }T
T{ 1 2 nip -> 2 }T
T{ 1 2 over -> 1 2 1 }T
T{ 1 2 3 rot -> 2 3 1 }T
T{ 1 2 3 -rot -> 3 1 2 }T
T{ 1 2 swap -> 2 1 }T

\ There is no formal ANS test for TUCK, this added 01. July 2018
T{ 2 1 tuck -> 1 2 1 }T

\ There is no formal ANS test for PICK, this added 01. July 2018
\ Note that ANS's PICK is different from FIG Forth PICK
T{ 1      0 pick -> 1 1 }T    \ Defined by standard: 0 PICK is same as DUP
T{ 1 2    1 pick -> 1 2 1 }T  \ Defined by standard: 1 PICK is same as OVER
T{ 1 2 3  2 pick -> 1 2 3 1 }T

\ ------------------------------------------------------------------------
testing >r r> r@ 2>r 2r> 2r@

T{ : gr1 >r r> ; -> }T
T{ : gr2 >r r@ r> drop ; -> }T
T{ 123 gr1 -> 123 }T
T{ 123 gr2 -> 123 }T
T{ 1s gr1 -> 1s }T \ return stack holds cells

\ There are no official ANS tests for 2>R, 2R>, or 2R@, added 22. June 2018
T{ : gr3 2>r 2r> ; -> }T
T{ : gr4 2>r 2r@ 2r> 2drop ; -> }T
T{ : gr5 2>r r> r> ; }T \ must reverse sequence, as 2r> is not r> r> 
T{ 123. gr3 -> 123. }T
T{ 123. gr4 -> 123. }T
T{ 123. gr5 -> 0 123 }T

\ ------------------------------------------------------------------------
testing add/subtract: + - 1+ 1- abs negate 

T{ 0 5 + -> 5 }T
T{ 5 0 + -> 5 }T
T{ 0 -5 + -> -5 }T
T{ -5 0 + -> -5 }T
T{ 1 2 + -> 3 }T
T{ 1 -2 + -> -1 }T
T{ -1 2 + -> 1 }T
T{ -1 -2 + -> -3 }T
T{ -1 1 + -> 0 }T
T{ mid-uint 1 + -> mid-uint+1 }T

T{ 0 5 - -> -5 }T
T{ 5 0 - -> 5 }T
T{ 0 -5 - -> 5 }T
T{ -5 0 - -> -5 }T
T{ 1 2 - -> -1 }T
T{ 1 -2 - -> 3 }T
T{ -1 2 - -> -3 }T
T{ -1 -2 - -> 1 }T
T{ 0 1 - -> -1 }T
T{ mid-uint+1 1 - -> mid-uint }T

T{ 0 1+ -> 1 }T
T{ -1 1+ -> 0 }T
T{ 1 1+ -> 2 }T
T{ mid-uint 1+ -> mid-uint+1 }T

T{ 2 1- -> 1 }T
T{ 1 1- -> 0 }T
T{ 0 1- -> -1 }T
T{ mid-uint+1 1- -> mid-uint }T

T{ 0 negate -> 0 }T
T{ 1 negate -> -1 }T
T{ -1 negate -> 1 }T
T{ 2 negate -> -2 }T
T{ -2 negate -> 2 }T

T{ 0 abs -> 0 }T
T{ 1 abs -> 1 }T
T{ -1 abs -> 1 }T
T{ min-int abs -> mid-uint+1 }T

\ ------------------------------------------------------------------------
testing multiply: s>d * m* um*

T{ 0 s>d -> 0 0 }T
T{ 1 s>d -> 1 0 }T
T{ 2 s>d -> 2 0 }T
T{ -1 s>d -> -1 -1 }T
T{ -2 s>d -> -2 -1 }T
T{ min-int s>d -> min-int -1 }T
T{ max-int s>d -> max-int 0 }T

T{ 0 0 m* -> 0 s>d }T
T{ 0 1 m* -> 0 s>d }T
T{ 1 0 m* -> 0 s>d }T
T{ 1 2 m* -> 2 s>d }T
T{ 2 1 m* -> 2 s>d }T
T{ 3 3 m* -> 9 s>d }T
T{ -3 3 m* -> -9 s>d }T
T{ 3 -3 m* -> -9 s>d }T
T{ -3 -3 m* -> 9 s>d }T
T{ 0 min-int m* -> 0 s>d }T
T{ 1 min-int m* -> min-int s>d }T
T{ 2 min-int m* -> 0 1s }T
T{ 0 max-int m* -> 0 s>d }T
T{ 1 max-int m* -> max-int s>d }T
T{ 2 max-int m* -> max-int 1 lshift 0 }T
T{ min-int min-int m* -> 0 msb 1 rshift }T
T{ max-int min-int m* -> msb msb 2/ }T
T{ max-int max-int m* -> 1 msb 2/ invert }T

T{ 0 0 * -> 0 }T \ test identities
T{ 0 1 * -> 0 }T
T{ 1 0 * -> 0 }T
T{ 1 2 * -> 2 }T
T{ 2 1 * -> 2 }T
T{ 3 3 * -> 9 }T
T{ -3 3 * -> -9 }T
T{ 3 -3 * -> -9 }T
T{ -3 -3 * -> 9 }T

T{ mid-uint+1 1 rshift 2 * -> mid-uint+1 }T
T{ mid-uint+1 2 rshift 4 * -> mid-uint+1 }T
T{ mid-uint+1 1 rshift mid-uint+1 or 2 * -> mid-uint+1 }T

T{ 0 0 um* -> 0 0 }T
T{ 0 1 um* -> 0 0 }T
T{ 1 0 um* -> 0 0 }T
T{ 1 2 um* -> 2 0 }T
T{ 2 1 um* -> 2 0 }T
T{ 3 3 um* -> 9 0 }T

T{ mid-uint+1 1 rshift 2 um* -> mid-uint+1 0 }T
T{ mid-uint+1 2 um* -> 0 1 }T
T{ mid-uint+1 4 um* -> 0 2 }T
T{ 1s 2 um* -> 1s 1 lshift 1 }T
T{ max-uint max-uint um* -> 1 1 invert }T

\ ------------------------------------------------------------------------
testing divide: fm/mod sm/rem um/mod */ */mod / /mod mod

T{ 0 s>d 1 fm/mod -> 0 0 }T
T{ 1 s>d 1 fm/mod -> 0 1 }T
T{ 2 s>d 1 fm/mod -> 0 2 }T
T{ -1 s>d 1 fm/mod -> 0 -1 }T
T{ -2 s>d 1 fm/mod -> 0 -2 }T
T{ 0 s>d -1 fm/mod -> 0 0 }T
T{ 1 s>d -1 fm/mod -> 0 -1 }T
T{ 2 s>d -1 fm/mod -> 0 -2 }T
T{ -1 s>d -1 fm/mod -> 0 1 }T
T{ -2 s>d -1 fm/mod -> 0 2 }T
T{ 2 s>d 2 fm/mod -> 0 1 }T
T{ -1 s>d -1 fm/mod -> 0 1 }T
T{ -2 s>d -2 fm/mod -> 0 1 }T
T{  7 s>d  3 fm/mod -> 1 2 }T
T{  7 s>d -3 fm/mod -> -2 -3 }T
T{ -7 s>d  3 fm/mod -> 2 -3 }T
T{ -7 s>d -3 fm/mod -> -1 2 }T
T{ max-int s>d 1 fm/mod -> 0 max-int }T
T{ min-int s>d 1 fm/mod -> 0 min-int }T
T{ max-int s>d max-int fm/mod -> 0 1 }T
T{ min-int s>d min-int fm/mod -> 0 1 }T
T{ 1s 1 4 fm/mod -> 3 max-int }T
T{ 1 min-int m* 1 fm/mod -> 0 min-int }T
T{ 1 min-int m* min-int fm/mod -> 0 1 }T
T{ 2 min-int m* 2 fm/mod -> 0 min-int }T
T{ 2 min-int m* min-int fm/mod -> 0 2 }T
T{ 1 max-int m* 1 fm/mod -> 0 max-int }T
T{ 1 max-int m* max-int fm/mod -> 0 1 }T
T{ 2 max-int m* 2 fm/mod -> 0 max-int }T
T{ 2 max-int m* max-int fm/mod -> 0 2 }T
T{ min-int min-int m* min-int fm/mod -> 0 min-int }T
T{ min-int max-int m* min-int fm/mod -> 0 max-int }T
T{ min-int max-int m* max-int fm/mod -> 0 min-int }T
T{ max-int max-int m* max-int fm/mod -> 0 max-int }T

T{ 0 s>d 1 sm/rem -> 0 0 }T
T{ 1 s>d 1 sm/rem -> 0 1 }T
T{ 2 s>d 1 sm/rem -> 0 2 }T
T{ -1 s>d 1 sm/rem -> 0 -1 }T
T{ -2 s>d 1 sm/rem -> 0 -2 }T
T{ 0 s>d -1 sm/rem -> 0 0 }T
T{ 1 s>d -1 sm/rem -> 0 -1 }T
T{ 2 s>d -1 sm/rem -> 0 -2 }T
T{ -1 s>d -1 sm/rem -> 0 1 }T
T{ -2 s>d -1 sm/rem -> 0 2 }T
T{ 2 s>d 2 sm/rem -> 0 1 }T
T{ -1 s>d -1 sm/rem -> 0 1 }T
T{ -2 s>d -2 sm/rem -> 0 1 }T
T{  7 s>d  3 sm/rem -> 1 2 }T
T{  7 s>d -3 sm/rem -> 1 -2 }T
T{ -7 s>d  3 sm/rem -> -1 -2 }T
T{ -7 s>d -3 sm/rem -> -1 2 }T
T{ max-int s>d 1 sm/rem -> 0 max-int }T
T{ min-int s>d 1 sm/rem -> 0 min-int }T
T{ max-int s>d max-int sm/rem -> 0 1 }T
T{ min-int s>d min-int sm/rem -> 0 1 }T
T{ 1s 1 4 sm/rem -> 3 max-int }T
T{ 2 min-int m* 2 sm/rem -> 0 min-int }T
T{ 2 min-int m* min-int sm/rem -> 0 2 }T
T{ 2 max-int m* 2 sm/rem -> 0 max-int }T
T{ 2 max-int m* max-int sm/rem -> 0 2 }T
T{ min-int min-int m* min-int sm/rem -> 0 min-int }T
T{ min-int max-int m* min-int sm/rem -> 0 max-int }T
T{ min-int max-int m* max-int sm/rem -> 0 min-int }T
T{ max-int max-int m* max-int sm/rem -> 0 max-int }T

T{ 0 0 1 um/mod -> 0 0 }T
T{ 1 0 1 um/mod -> 0 1 }T
T{ 1 0 2 um/mod -> 1 0 }T
T{ 3 0 2 um/mod -> 1 1 }T
T{ max-uint 2 um* 2 um/mod -> 0 max-uint }T
T{ max-uint 2 um* max-uint um/mod -> 0 2 }T
T{ max-uint max-uint um* max-uint um/mod -> 0 max-uint }T

: iffloored
   [ -3 2 / -2 = invert ] literal if postpone \ then ;
: ifsym
   [ -3 2 / -1 = invert ] literal if postpone \ then ;

\ the system might do either floored or symmetric division.
\ since we have already tested m*, fm/mod, and sm/rem we can use them in test.
iffloored : t/mod  >r s>d r> fm/mod ;
iffloored : t/     t/mod swap drop ;
iffloored : tmod   t/mod drop ;
iffloored : t*/mod >r m* r> fm/mod ;
iffloored : t*/    t*/mod swap drop ;
ifsym     : t/mod  >r s>d r> sm/rem ;
ifsym     : t/     t/mod swap drop ;
ifsym     : tmod   t/mod drop ;
ifsym     : t*/mod >r m* r> sm/rem ;
ifsym     : t*/    t*/mod swap drop ;

T{ 0 1 /mod -> 0 1 t/mod }T
T{ 1 1 /mod -> 1 1 t/mod }T
T{ 2 1 /mod -> 2 1 t/mod }T
T{ -1 1 /mod -> -1 1 t/mod }T
T{ -2 1 /mod -> -2 1 t/mod }T
T{ 0 -1 /mod -> 0 -1 t/mod }T
T{ 1 -1 /mod -> 1 -1 t/mod }T
T{ 2 -1 /mod -> 2 -1 t/mod }T
T{ -1 -1 /mod -> -1 -1 t/mod }T
T{ -2 -1 /mod -> -2 -1 t/mod }T
T{ 2 2 /mod -> 2 2 t/mod }T
T{ -1 -1 /mod -> -1 -1 t/mod }T
T{ -2 -2 /mod -> -2 -2 t/mod }T
T{ 7 3 /mod -> 7 3 t/mod }T
T{ 7 -3 /mod -> 7 -3 t/mod }T
T{ -7 3 /mod -> -7 3 t/mod }T
T{ -7 -3 /mod -> -7 -3 t/mod }T
T{ max-int 1 /mod -> max-int 1 t/mod }T
T{ min-int 1 /mod -> min-int 1 t/mod }T
T{ max-int max-int /mod -> max-int max-int t/mod }T
T{ min-int min-int /mod -> min-int min-int t/mod }T

T{ 0 1 / -> 0 1 t/ }T
T{ 1 1 / -> 1 1 t/ }T
T{ 2 1 / -> 2 1 t/ }T
T{ -1 1 / -> -1 1 t/ }T
T{ -2 1 / -> -2 1 t/ }T
T{ 0 -1 / -> 0 -1 t/ }T
T{ 1 -1 / -> 1 -1 t/ }T
T{ 2 -1 / -> 2 -1 t/ }T
T{ -1 -1 / -> -1 -1 t/ }T
T{ -2 -1 / -> -2 -1 t/ }T
T{ 2 2 / -> 2 2 t/ }T
T{ -1 -1 / -> -1 -1 t/ }T
T{ -2 -2 / -> -2 -2 t/ }T
T{ 7 3 / -> 7 3 t/ }T
T{ 7 -3 / -> 7 -3 t/ }T
T{ -7 3 / -> -7 3 t/ }T
T{ -7 -3 / -> -7 -3 t/ }T
T{ max-int 1 / -> max-int 1 t/ }T
T{ min-int 1 / -> min-int 1 t/ }T
T{ max-int max-int / -> max-int max-int t/ }T
T{ min-int min-int / -> min-int min-int t/ }T

T{ 0 1 mod -> 0 1 tmod }T
T{ 1 1 mod -> 1 1 tmod }T
T{ 2 1 mod -> 2 1 tmod }T
T{ -1 1 mod -> -1 1 tmod }T
T{ -2 1 mod -> -2 1 tmod }T
T{ 0 -1 mod -> 0 -1 tmod }T
T{ 1 -1 mod -> 1 -1 tmod }T
T{ 2 -1 mod -> 2 -1 tmod }T
T{ -1 -1 mod -> -1 -1 tmod }T
T{ -2 -1 mod -> -2 -1 tmod }T
T{ 2 2 mod -> 2 2 tmod }T
T{ -1 -1 mod -> -1 -1 tmod }T
T{ -2 -2 mod -> -2 -2 tmod }T
T{ 7 3 mod -> 7 3 tmod }T
T{ 7 -3 mod -> 7 -3 tmod }T
T{ -7 3 mod -> -7 3 tmod }T
T{ -7 -3 mod -> -7 -3 tmod }T
T{ max-int 1 mod -> max-int 1 tmod }T
T{ min-int 1 mod -> min-int 1 tmod }T
T{ max-int max-int mod -> max-int max-int tmod }T
T{ min-int min-int mod -> min-int min-int tmod }T

T{ 0 2 1 */ -> 0 2 1 t*/ }T
T{ 1 2 1 */ -> 1 2 1 t*/ }T
T{ 2 2 1 */ -> 2 2 1 t*/ }T
T{ -1 2 1 */ -> -1 2 1 t*/ }T
T{ -2 2 1 */ -> -2 2 1 t*/ }T
T{ 0 2 -1 */ -> 0 2 -1 t*/ }T
T{ 1 2 -1 */ -> 1 2 -1 t*/ }T
T{ 2 2 -1 */ -> 2 2 -1 t*/ }T
T{ -1 2 -1 */ -> -1 2 -1 t*/ }T
T{ -2 2 -1 */ -> -2 2 -1 t*/ }T
T{ 2 2 2 */ -> 2 2 2 t*/ }T
T{ -1 2 -1 */ -> -1 2 -1 t*/ }T
T{ -2 2 -2 */ -> -2 2 -2 t*/ }T
T{ 7 2 3 */ -> 7 2 3 t*/ }T
T{ 7 2 -3 */ -> 7 2 -3 t*/ }T
T{ -7 2 3 */ -> -7 2 3 t*/ }T
T{ -7 2 -3 */ -> -7 2 -3 t*/ }T
T{ max-int 2 max-int */ -> max-int 2 max-int t*/ }T
T{ min-int 2 min-int */ -> min-int 2 min-int t*/ }T

T{ 0 2 1 */mod -> 0 2 1 t*/mod }T
T{ 1 2 1 */mod -> 1 2 1 t*/mod }T
T{ 2 2 1 */mod -> 2 2 1 t*/mod }T
T{ -1 2 1 */mod -> -1 2 1 t*/mod }T
T{ -2 2 1 */mod -> -2 2 1 t*/mod }T
T{ 0 2 -1 */mod -> 0 2 -1 t*/mod }T
T{ 1 2 -1 */mod -> 1 2 -1 t*/mod }T
T{ 2 2 -1 */mod -> 2 2 -1 t*/mod }T
T{ -1 2 -1 */mod -> -1 2 -1 t*/mod }T
T{ -2 2 -1 */mod -> -2 2 -1 t*/mod }T
T{ 2 2 2 */mod -> 2 2 2 t*/mod }T
T{ -1 2 -1 */mod -> -1 2 -1 t*/mod }T
T{ -2 2 -2 */mod -> -2 2 -2 t*/mod }T
T{ 7 2 3 */mod -> 7 2 3 t*/mod }T
T{ 7 2 -3 */mod -> 7 2 -3 t*/mod }T
T{ -7 2 3 */mod -> -7 2 3 t*/mod }T
T{ -7 2 -3 */mod -> -7 2 -3 t*/mod }T
T{ max-int 2 max-int */mod -> max-int 2 max-int t*/mod }T
T{ min-int 2 min-int */mod -> min-int 2 min-int t*/mod }T

\ ------------------------------------------------------------------------
testing here , @ ! cell+ cells c, c@ c! char+ chars 2@ 2! align aligned +! allot pad unused compile,

decimal
here 1 allot
here
9 allot                     \ Growing by 9 and shrinking
-10 allot                    \ by 10 should bring us back
here                        \ to where we started.
constant 3rda
constant 2nda
constant 1sta
T{ 1sta 2nda u< -> <true> }T  \ here must grow with allot ...
T{ 1sta 1+ -> 2nda }T         \ ... by one address unit
T{ 3rda -> 1sta }T            \ and shrink back to the beginning.
hex

here 1 ,
here 2 ,
constant 2nd
constant 1st
T{ 1st 2nd u< -> <true> }T \ here must grow with allot ...
T{ 1st cell+ -> 2nd }T     \ ... by one cell (test for char+)
T{ 1st 1 cells + -> 2nd }T
T{ 1st @ 2nd @ -> 1 2 }T
T{ 5 1st ! -> }T
T{ 1st @ 2nd @ -> 5 2 }T
T{ 6 2nd ! -> }T
T{ 1st @ 2nd @ -> 5 6 }T
T{ 1st 2@ -> 6 5 }T
T{ 2 1 1st 2! -> }T
T{ 1st 2@ -> 2 1 }T
T{ 1s 1st !  1st @ -> 1s }T  \ can store cell-wide value

here 1 c,
here 2 c,
constant 2ndc
constant 1stc
T{ 1stc 2ndc u< -> <true> }T \ here must grow with allot
T{ 1stc char+ -> 2ndc }T     \ ... by one char
T{ 1stc 1 chars + -> 2ndc }T
T{ 1stc c@ 2ndc c@ -> 1 2 }T
T{ 3 1stc c! -> }T
T{ 1stc c@ 2ndc c@ -> 3 2 }T
T{ 4 2ndc c! -> }T
T{ 1stc c@ 2ndc c@ -> 3 4 }T

align 1 allot here align here 3 cells allot
constant a-addr  constant ua-addr
T{ ua-addr aligned -> a-addr }T
T{ 1 a-addr c!  a-addr c@ -> 1 }T
T{ 1234 a-addr  !  a-addr  @ -> 1234 }T
T{ 123 456 a-addr 2!  a-addr 2@ -> 123 456 }T
T{ 2 a-addr char+ c!  a-addr char+ c@ -> 2 }T
T{ 3 a-addr cell+ c!  a-addr cell+ c@ -> 3 }T
T{ 1234 a-addr cell+ !  a-addr cell+ @ -> 1234 }T
T{ 123 456 a-addr cell+ 2!  a-addr cell+ 2@ -> 123 456 }T

: bits ( x -- u )
   0 swap begin
   dup while 
      dup msb and if
         >r 1+ r> 
      then 2* 
   repeat 
   drop ;

( characters >= 1 au, <= size of cell, >= 8 bits )
T{ 1 chars 1 < -> <false> }T
T{ 1 chars 1 cells > -> <false> }T
( TODO how to find number of bits? )

( cells >= 1 au, integral multiple of char size, >= 16 bits )
T{ 1 cells 1 < -> <false> }T
T{ 1 cells 1 chars mod -> 0 }T
T{ 1s bits 10 < -> <false> }T

T{ 0 1st ! -> }T
T{ 1 1st +! -> }T
T{ 1st @ -> 1 }T
T{ -1 1st +! 1st @ -> 0 }T

( here + unused + buffer size must be total RAM, that is, $7FFF )
T{ pad here - -> FF }T \ PAD must have offset of $FF
T{ here unused + 3FF + -> 7FFF }T

:noname dup + ; constant dup+ 
T{ : q dup+ compile, ; -> }T 
T{ : as [ q ] ; -> }T 
T{ 123 as -> 246 }T

\ ------------------------------------------------------------------------
testing char [char] [ ] bl s"

T{ bl -> 20 }T
T{ char X -> 58 }T
T{ char HELLO -> 48 }T
T{ : gc1 [char] X ; -> }T
T{ : gc2 [char] HELLO ; -> }T
T{ gc1 -> 58 }T
T{ gc2 -> 48 }T
T{ : gc3 [ gc1 ] literal ; -> }T
T{ gc3 -> 58 }T
T{ : gc4 s" XY" ; -> }T
T{ gc4 swap drop -> 2 }T
T{ gc4 drop dup c@ swap char+ c@ -> 58 59 }T

\ ------------------------------------------------------------------------
testing ' ['] find execute immediate count literal postpone state

T{ : gt1 123 ; -> }T
T{ ' gt1 execute -> 123 }T
T{ : gt2 ['] gt1 ; immediate -> }T
T{ gt2 execute -> 123 }T
here 3 c, char g c, char t c, char 1 c, constant gt1string
here 3 c, char g c, char t c, char 2 c, constant gt2string
T{ gt1string find -> ' gt1 -1 }T
T{ gt2string find -> ' gt2 1 }T
( TODO how to search for non-existent word? )
T{ : gt3 gt2 literal ; -> }T
T{ gt3 -> ' gt1 }T
T{ gt1string count -> gt1string char+ 3 }T

T{ : gt4 postpone gt1 ; immediate -> }T
T{ : gt5 gt4 ; -> }T
T{ gt5 -> 123 }T
T{ : gt6 345 ; immediate -> }T
T{ : gt7 postpone gt6 ; -> }T
T{ gt7 -> 345 }T

T{ : gt8 state @ ; immediate -> }T
T{ gt8 -> 0 }T
T{ : gt9 gt8 literal ; -> }T
T{ gt9 0= -> <false> }T

\ ------------------------------------------------------------------------
testing if else then begin while repeat until recurse

T{ : gi1 if 123 then ; -> }T
T{ : gi2 if 123 else 234 then ; -> }T
T{ 0 gi1 -> }T
T{ 1 gi1 -> 123 }T
T{ -1 gi1 -> 123 }T
T{ 0 gi2 -> 234 }T
T{ 1 gi2 -> 123 }T
T{ -1 gi1 -> 123 }T

T{ : gi3 begin dup 5 < while dup 1+ repeat ; -> }T
T{ 0 gi3 -> 0 1 2 3 4 5 }T
T{ 4 gi3 -> 4 5 }T
T{ 5 gi3 -> 5 }T
T{ 6 gi3 -> 6 }T

T{ : gi4 begin dup 1+ dup 5 > until ; -> }T
T{ 3 gi4 -> 3 4 5 6 }T
T{ 5 gi4 -> 5 6 }T
T{ 6 gi4 -> 6 7 }T

T{ : gi5 begin dup 2 > while dup 5 < while dup 1+ repeat 123 else 345 then ; -> }T
T{ 1 gi5 -> 1 345 }T
T{ 2 gi5 -> 2 345 }T
T{ 3 gi5 -> 3 4 5 123 }T
T{ 4 gi5 -> 4 5 123 }T
T{ 5 gi5 -> 5 123 }T

T{ : gi6 ( n -- 0,1,..n ) dup if dup >r 1- recurse r> then ; -> }T
T{ 0 gi6 -> 0 }T
T{ 1 gi6 -> 0 1 }T
T{ 2 gi6 -> 0 1 2 }T
T{ 3 gi6 -> 0 1 2 3 }T
T{ 4 gi6 -> 0 1 2 3 4 }T

decimal
T{ :noname ( n -- 0, 1, .., n ) 
     dup if dup >r 1- recurse r> then 
   ; 
   constant rn1 -> }T
T{ 0 rn1 execute -> 0 }T
T{ 4 rn1 execute -> 0 1 2 3 4 }T

:noname ( n -- n1 )
   1- dup
   case 0 of exit endof
     1 of 11 swap recurse endof
     2 of 22 swap recurse endof
     3 of 33 swap recurse endof
     drop abs recurse exit
   endcase
; constant rn2

T{  1 rn2 execute -> 0 }T
T{  2 rn2 execute -> 11 0 }T
T{  4 rn2 execute -> 33 22 11 0 }T
T{ 25 rn2 execute -> 33 22 11 0 }T
hex

\ ------------------------------------------------------------------------
testing case of endof endcase

: cs1 case 
   1 of 111 endof
   2 of 222 endof
   3 of 333 endof
   4 of 444 endof
   5 of 555 endof
   6 of 666 endof
   7 of 777 endof
   >r 999 r>
   endcase
;

T{ 1 cs1 -> 111 }T
T{ 2 cs1 -> 222 }T
T{ 3 cs1 -> 333 }T
T{ 4 cs1 -> 444 }T
T{ 5 cs1 -> 555 }T
T{ 6 cs1 -> 666 }T
T{ 7 cs1 -> 777 }T
T{ 8 cs1 -> 999 }T \ default

: cs2 >r case

   -1 of case r@ 1 of 100 endof
                2 of 200 endof
                >r -300 r>
        endcase
     endof
   -2 of case r@ 1 of -99 endof
                >r -199 r>
        endcase
     endof
     >r 299 r>
   endcase r> drop ;

T{ -1 1 cs2 ->  100 }T
T{ -1 2 cs2 ->  200 }T
T{ -1 3 cs2 -> -300 }T
T{ -2 1 cs2 ->  -99 }T
T{ -2 2 cs2 -> -199 }T
T{  0 2 cs2 ->  299 }T

\ ------------------------------------------------------------------------
testing do loop +loop i j unloop leave exit ?do

T{ : gd1 do i loop ; -> }T
T{ 4 1 gd1 -> 1 2 3 }T
T{ 2 -1 gd1 -> -1 0 1 }T
T{ mid-uint+1 mid-uint gd1 -> mid-uint }T

T{ : gd2 do i -1 +loop ; -> }T
T{ 1 4 gd2 -> 4 3 2 1 }T
T{ -1 2 gd2 -> 2 1 0 -1 }T
T{ mid-uint mid-uint+1 gd2 -> mid-uint+1 mid-uint }T

T{ : gd3 do 1 0 do j loop loop ; -> }T
T{ 4 1 gd3 -> 1 2 3 }T
T{ 2 -1 gd3 -> -1 0 1 }T
T{ mid-uint+1 mid-uint gd3 -> mid-uint }T

T{ : gd4 do 1 0 do j loop -1 +loop ; -> }T
T{ 1 4 gd4 -> 4 3 2 1 }T
T{ -1 2 gd4 -> 2 1 0 -1 }T
T{ mid-uint mid-uint+1 gd4 -> mid-uint+1 mid-uint }T

T{ : gd5 123 swap 0 do i 4 > if drop 234 leave then loop ; -> }T
T{ 1 gd5 -> 123 }T
T{ 5 gd5 -> 123 }T
T{ 6 gd5 -> 234 }T

T{ : gd6  ( pat: T{0 0}T,T{0 0}TT{1 0}TT{1 1}T,T{0 0}TT{1 0}TT{1 1}TT{2 0}TT{2 1}TT{2 2}T )
   0 swap 0 do
      i 1+ 0 do i j + 3 = if i unloop i unloop exit then 1+ loop
    loop ; -> }T
T{ 1 gd6 -> 1 }T
T{ 2 gd6 -> 3 }T
T{ 3 gd6 -> 4 1 2 }T

: qd ?do i loop ; 
T{   789   789 qd -> }T 
T{ -9876 -9876 qd -> }T 
T{     5     0 qd -> 0 1 2 3 4 }T

: qd1 ?do i 10 +loop ; 
T{ 50 1 qd1 -> 1 11 21 31 41 }T 
T{ 50 0 qd1 -> 0 10 20 30 40 }T

: qd2 ?do i 3 > if leave else i then loop ; 
T{ 5 -1 qd2 -> -1 0 1 2 3 }T

: qd3 ?do i 1 +loop ; 
T{ 4  4 qd3 -> }T 
T{ 4  1 qd3 ->  1 2 3 }T
T{ 2 -1 qd3 -> -1 0 1 }T

: qd4 ?do i -1 +loop ; 
T{  4 4 qd4 -> }T
T{  1 4 qd4 -> 4 3 2  1 }T 
T{ -1 2 qd4 -> 2 1 0 -1 }T

: qd5 ?do i -10 +loop ; 
T{   1 50 qd5 -> 50 40 30 20 10   }T 
T{   0 50 qd5 -> 50 40 30 20 10 0 }T 
T{ -25 10 qd5 -> 10 0 -10 -20     }T

variable qditerations 
variable qdincrement

: qd6 ( limit start increment -- )    qdincrement ! 
   0 qditerations ! 
   ?do 
     1 qditerations +! 
     i 
     qditerations @ 6 = if leave then 
     qdincrement @ 
   +loop qditerations @ 
;

T{  4  4 -1 qd6 ->                   0  }T 
T{  1  4 -1 qd6 ->  4  3  2  1       4  }T 
T{  4  1 -1 qd6 ->  1  0 -1 -2 -3 -4 6  }T 
T{  4  1  0 qd6 ->  1  1  1  1  1  1 6  }T 
T{  0  0  0 qd6 ->                   0  }T 
T{  1  4  0 qd6 ->  4  4  4  4  4  4 6  }T 
T{  1  4  1 qd6 ->  4  5  6  7  8  9 6  }T 
T{  4  1  1 qd6 ->  1  2  3          3  }T 
T{  4  4  1 qd6 ->                   0  }T 
T{  2 -1 -1 qd6 -> -1 -2 -3 -4 -5 -6 6  }T 
T{ -1  2 -1 qd6 ->  2  1  0 -1       4  }T 
T{  2 -1  0 qd6 -> -1 -1 -1 -1 -1 -1 6  }T 
T{ -1  2  0 qd6 ->  2  2  2  2  2  2 6  }T 
T{ -1  2  1 qd6 ->  2  3  4  5  6  7 6  }T 
T{  2 -1  1 qd6 -> -1  0  1          3  }T

\ ------------------------------------------------------------------------
testing defining words: : ; constant variable create does> >body value to

T{ 123 constant x123 -> }T
T{ x123 -> 123 }T
T{ : equ constant ; -> }T
T{ x123 equ y123 -> }T
T{ y123 -> 123 }T

T{ variable v1 -> }T
T{ 123 v1 ! -> }T
T{ v1 @ -> 123 }T

T{ : nop : postpone ; ; -> }T
T{ nop nop1 nop nop2 -> }T
T{ nop1 -> }T
T{ nop2 -> }T

T{ : does1 does> @ 1 + ; -> }T
T{ : does2 does> @ 2 + ; -> }T
T{ create cr1 -> }T
T{ cr1 -> here }T
T{ ' cr1 >body -> here }T
T{ 1 , -> }T
T{ cr1 @ -> 1 }T
T{ does1 -> }T
T{ cr1 -> 2 }T
T{ does2 -> }T
T{ cr1 -> 3 }T

\ The following test is not part of the original suite, but belongs
\ to the "weird:" test following it. See discussion at
\ https://github.com/scotws/TaliForth2/issues/61
T{ : odd: create does> 1 + ; -> }T
T{ odd: o1 -> }T
T{ ' o1 >body -> here }T
T{ o1 -> here 1 + }T

T{ : weird: create does> 1 + does> 2 + ; -> }T
T{ weird: w1 -> }T
T{ ' w1 >body -> here }T
T{ w1 -> here 1 + }T
T{ w1 -> here 2 + }T

T{  111 value v1 -> }T
T{ -999 value v2 -> }T
T{ v1 ->  111 }T
T{ v2 -> -999 }T 
T{ 222 to v1 -> }T 
T{ v1 -> 222 }T
T{ : vd1 v1 ; -> }T
T{ vd1 -> 222 }T

T{ : vd2 to v2 ; -> }T
T{ v2 -> -999 }T
T{ -333 vd2 -> }T
T{ v2 -> -333 }T
T{ v1 ->  222 }T

\ ------------------------------------------------------------------------
testing evaluate

: ge1 s" 123" ; immediate
: ge2 s" 123 1+" ; immediate
: ge3 s" : ge4 345 ;" ;
: ge5 evaluate ; immediate

T{ ge1 evaluate -> 123 }T \ test evaluate in interp. state
T{ ge2 evaluate -> 124 }T
T{ ge3 evaluate -> }T
T{ ge4 -> 345 }T

T{ : ge6 ge1 ge5 ; -> }T  \ test evaluate in compile state
T{ ge6 -> 123 }T
T{ : ge7 ge2 ge5 ; -> }T
T{ ge7 -> 124 }T

\ ------------------------------------------------------------------------
testing source >in word

: gs1 s" source" 2dup evaluate 
       >r swap >r = r> r> = ;
T{ gs1 -> <true> <true> }T

variable scans
: rescan?  -1 scans +!
   scans @ if
      0 >in !
   then ;

T{ 2 scans !  
345 rescan?  
-> 345 345 }T

: gs2  5 scans ! s" 123 rescan?" evaluate ;
T{ gs2 -> 123 123 123 123 123 }T

: gs3 word count swap c@ ;
T{ bl gs3 hello -> 5 char h }T
T{ char " gs3 goodbye" -> 7 char g }T
T{ bl gs3 
drop -> 0 }T \ blank line return zero-length string

: gs4 source >in ! drop ;
T{ gs4 123 456 
-> }T

\ ------------------------------------------------------------------------
testing <# # #s #> hold sign base >number hex decimal
hex

\ compare two strings.
: s=  ( addr1 c1 addr2 c2 -- t/f ) 
   >r swap r@ = if \ make sure strings have same length
      r> ?dup if   \ if non-empty strings
         0 do
            over c@ over c@ - if
               2drop <false> unloop exit
            then
            swap char+ swap char+
         loop
      then
      2drop <true> \ if we get here, strings match
   else
      r> drop 2drop <false>  \ lengths mismatch
   then ;

: gp1  <# 41 hold 42 hold 0 0 #> s" BA" s= ;
T{ gp1 -> <true> }T

: gp2  <# -1 sign 0 sign -1 sign 0 0 #> s" --" s= ;
T{ gp2 -> <true> }T

: gp3  <# 1 0 # # #> s" 01" s= ;
T{ gp3 -> <true> }T

: gp4  <# 1 0 #s #> s" 1" s= ;
T{ gp4 -> <true> }T

24 constant max-base   \ base 2 .. 36
max-base .( max-base post def: ) . cr  ( TODO TEST )
: count-bits
   0 0 invert 
   begin 
      dup while
      >r 1+ r> 2* 
   repeat 
   drop ;

count-bits 2* constant #bits-ud  \ number of bits in ud

: gp5
   base @ <true>
   max-base 1+ 2 do   \ for each possible base
      i base !    \ tbd: assumes base works
      i 0 <# #s #> s" 10" s= and
   loop
   swap base ! ;
T{ gp5 -> <true> }T

: gp6
   base @ >r  2 base !
   max-uint max-uint <# #s #>  \ maximum ud to binary
   r> base !    \ s: c-addr u
   dup #bits-ud = swap
   0 do     \ s: c-addr flag
      over c@ [char] 1 = and  \ all ones
      >r char+ r>
   loop swap drop ;
T{ gp6 -> <true> }T


\ Split up long testing word from ANS Forth in two parts
\ to figure out what is wrong

\ Test the numbers 0 to 15 in max-base
: gp7-1
   base @ >r  
   max-base base !
   <true>

   a 0 do
      i 0 <# #s #>
      1 = swap c@ i 30 + = and and
   loop
   
   r> base ! ;

T{ gp7-1 -> <true> }T

\ Test the numbers 16 to max-base in max-base
: gp7-2
   base @ >r  
   max-base base !
   <true>

   max-base a do
      i 0 <# #s #>
      2dup type cr ( TODO TEST )
      1 = swap c@ 41 i a - + = and and
      .s cr ( TODO TEST )
   loop

   r> base ! ;

T{ gp7-2 -> <true> }T

\ >number tests
create gn-buf 0 c,
: gn-string gn-buf 1 ;
: gn-consumed gn-buf char+ 0 ;
: gn'  [char] ' word char+ c@ gn-buf c!  gn-string ;

T{ 0 0 gn' 0' >number -> 0 0 gn-consumed }T
T{ 0 0 gn' 1' >number -> 1 0 gn-consumed }T
T{ 1 0 gn' 1' >number -> base @ 1+ 0 gn-consumed }T
T{ 0 0 gn' -' >number -> 0 0 gn-string }T \ should fail to convert these
T{ 0 0 gn' +' >number -> 0 0 gn-string }T
T{ 0 0 gn' .' >number -> 0 0 gn-string }T

: >number-based  base @ >r base ! >number r> base ! ;

T{ 0 0 gn' 2' 10 >number-based -> 2 0 gn-consumed }T
T{ 0 0 gn' 2'  2 >number-based -> 0 0 gn-string }T
T{ 0 0 gn' f' 10 >number-based -> f 0 gn-consumed }T
T{ 0 0 gn' g' 10 >number-based -> 0 0 gn-string }T
T{ 0 0 gn' g' max-base >number-based -> 10 0 gn-consumed }T
T{ 0 0 gn' z' max-base >number-based -> 23 0 gn-consumed }T

\ ud should equal ud' and len should be zero.
: gn1  ( ud base -- ud' len ) 
   base @ >r base !
   <# #s #>
   0 0 2swap >number swap drop  \ return length only
   r> base ! ;

T{ 0 0 2 gn1 -> 0 0 0 }T
T{ max-uint 0 2 gn1 -> max-uint 0 0 }T
T{ max-uint dup 2 gn1 -> max-uint dup 0 }T

T{ 0 0 max-base gn1 -> 0 0 0 }T
T{ max-uint 0 max-base gn1 -> max-uint 0 0 }T
T{ max-uint dup max-base gn1 -> max-uint dup 0 }T

: gn2 ( -- 16 10 )
   base @ >r  hex base @  decimal base @  r> base ! ;

T{ gn2 -> 10 a }T

\ ------------------------------------------------------------------------
testing action-of defer defer! defer@ is

T{ defer defer1 -> }T
T{ : action-defer1 action-of defer1 ; -> }T
T{ ' * ' defer1 defer! ->   }T
T{          2 3 defer1 -> 6 }T
T{ action-of defer1 -> ' * }T 
T{    action-defer1 -> ' * }T

T{ ' + is defer1 ->   }T
T{    1 2 defer1 -> 3 }T 
T{ action-of defer1 -> ' + }T
T{    action-defer1 -> ' + }T

T{ defer defer2 ->   }T 
T{ ' * ' defer2 defer! -> }T
T{   2 3 defer2 -> 6 }T
T{ ' + is defer2 ->   }T
T{    1 2 defer2 -> 3 }T

T{ defer defer3 -> }T
T{ ' * ' defer3 defer! -> }T
T{ 2 3 defer3 -> 6 }T
T{ ' + ' defer3 defer! -> }T
T{ 1 2 defer3 -> 3 }T

T{ defer defer4 -> }T
T{ ' * ' defer4 defer! -> }T
T{ 2 3 defer4 -> 6 }T
T{ ' defer4 defer@ -> ' * }T

T{ ' + is defer4 -> }T 
T{ 1 2 defer4 -> 3 }T 
T{ ' defer4 defer@ -> ' + }T

T{ defer defer5 -> }T
T{ : is-defer5 is defer5 ; -> }T
T{ ' * is defer5 -> }T
T{ 2 3 defer5 -> 6 }T
T{ ' + is-defer5 -> }T 
T{ 1 2 defer5 -> 3 }T

\ ------------------------------------------------------------------------
testing fill move

create fbuf 00 c, 00 c, 00 c,
create sbuf 12 c, 34 c, 56 c,
: seebuf fbuf c@  fbuf char+ c@  fbuf char+ char+ c@ ;

T{ fbuf 0 20 fill -> }T
T{ seebuf -> 00 00 00 }T

T{ fbuf 1 20 fill -> }T
T{ seebuf -> 20 00 00 }T

T{ fbuf 3 20 fill -> }T
T{ seebuf -> 20 20 20 }T

T{ fbuf fbuf 3 chars move -> }T  \ bizarre special case
T{ seebuf -> 20 20 20 }T

T{ sbuf fbuf 0 chars move -> }T
T{ seebuf -> 20 20 20 }T

T{ sbuf fbuf 1 chars move -> }T
T{ seebuf -> 12 20 20 }T

T{ sbuf fbuf 3 chars move -> }T
T{ seebuf -> 12 34 56 }T

T{ fbuf fbuf char+ 2 chars move -> }T
T{ seebuf -> 12 12 34 }T

T{ fbuf char+ fbuf 2 chars move -> }T
T{ seebuf -> 12 34 34 }T

\ CMOVE and CMOVE> propogation tests taken from 
\ https://forth-standard.org/standard/string/CMOVE and .../CMOVEtop
decimal
create cmbuf  97 c, 98 c, 99 c, 100 c, \ "abcd"
: seecmbuf  cmbuf c@  cmbuf char+ c@  cmbuf char+ char+ c@  cmbuf char+ char+ char+ c@ ;
T{ cmbuf dup char+ 3 cmove -> }T
T{ seecmbuf -> 97 97 97 97 }T \ "aaaa"

create cmubuf  97 c, 98 c, 99 c, 100 c, \ "abcd"
: seecmubuf  cmubuf c@  cmubuf char+ c@  cmubuf char+ char+ c@  cmubuf char+ char+ char+ c@ ;
T{ cmubuf dup char+ swap 3 cmove> -> }T
T{ seecmubuf -> 100 100 100 100 }T \ "dddd"

\ ------------------------------------------------------------------------
testing output: . ." cr emit space spaces type u.
hex

: output-test
   ." you should see the standard graphic characters:" cr
   41 bl do i emit loop cr
   61 41 do i emit loop cr
   7f 61 do i emit loop cr
   ." you should see 0-9 separated by a space:" cr
   9 1+ 0 do i . loop cr
   ." you should see 0-9 (with no spaces):" cr
   [char] 9 1+ [char] 0 do i 0 spaces emit loop cr
   ." you should see a-g separated by a space:" cr
   [char] g 1+ [char] a do i emit space loop cr
   ." you should see 0-5 separated by two spaces:" cr
   5 1+ 0 do i [char] 0 + emit 2 spaces loop cr
   ." you should see two separate lines:" cr
   s" line 1" type cr s" line 2" type cr
   ." you should see the number ranges of signed and unsigned numbers:" cr
   ."   signed: " min-int . max-int . cr
   ." unsigned: " 0 u. max-uint u. cr
;

T{ output-test -> }T

\ ------------------------------------------------------------------------
testing parse-name marker erase

\ Careful editing these, whitespace is significant
T{ parse-name abcd s" abcd" s= -> <true> }T 
T{ parse-name   abcde   s" abcde" s= -> <true> }T \ test empty parse area 
T{ parse-name 
   nip -> 0 }T    \ empty line 
T{ parse-name    
   nip -> 0 }T    \ line with white space
T{ : parse-name-test ( "name1" "name2" -- n ) 
   parse-name parse-name s= ; -> }T
T{ parse-name-test abcd abcd -> <true> }T 
T{ parse-name-test  abcd   abcd   -> <true> }T 
T{ parse-name-test abcde abcdf -> <false> }T 
T{ parse-name-test abcdf abcde -> <false> }T 
T{ parse-name-test abcde abcde 
    -> <true> }T 
T{ parse-name-test abcde abcde  
    -> <true> }T    \ line with white space

\ There is no official ANS test for MARKER, added 22. June 2018
\ TODO There is currently no test for FIND-NAME, taking it on faith here
T{ variable marker_size -> }T
T{ unused marker_size ! -> }T
T{ marker quarian -> }T
: marker_test ." Bosh'tet!" ;
T{ marker_test -> }T \ should print "Bosh'tet!"
T{ quarian -> }T 
T{ parse-name marker_test find-name -> 0 }T 
T{ marker_size @ unused = -> <true> }T

\ There is no official ANS test of ERASE, added 01. July 2018
T{ create erase_test -> }T
T{ 9 c, 1 c, 2 c, 3 c, 9 c, -> }T
T{ erase_test 1+ 3 erase -> }T  \ Erase bytes between 9 
T{ erase_test            c@ 9 = -> <true> }T
T{ erase_test 1 chars +  c@ 0 = -> <true> }T
T{ erase_test 2 chars +  c@ 0 = -> <true> }T
T{ erase_test 3 chars +  c@ 0 = -> <true> }T
T{ erase_test 4 chars +  c@ 9 = -> <true> }T


\ ------------------------------------------------------------------------
testing environment

\ This is from the ANS Forth specification at 
\ https://forth-standard.org/standard/core/ENVIRONMENTq but the first
\ test is commented out because it doesn't seem to make sense
\ T{ s" x:deferred" environment? dup 0= xor invert -> <true>  }T ( Huh? Why true? )
T{ s" x:notfound" environment? dup 0= xor invert -> <false> }T

\ These were added for Tali Forth 10. Aug 2018
hex
T{ s" /COUNTED-STRING"    environment? ->    7FFF <true> }T
T{ s" /HOLD"              environment? ->      FF <true> }T
T{ s" /PAD"               environment? ->      54 <true> }T
T{ s" ADDRESS-UNIT-BITS"  environment? ->       8 <true> }T
T{ s" FLOORED"            environment? -> <false> <true> }T
T{ s" MAX-CHAR"           environment? ->      FF <true> }T
T{ s" MAX-N"              environment? ->    7FFF <true> }T
T{ s" MAX-U"              environment? ->    FFFF <true> }T
T{ s" RETURN-STACK-CELLS" environment? ->      80 <true> }T
T{ s" STACK-CELLS"        environment? ->      20 <true> }T

T{ s" MAX-D"  environment? -> 7FFFFFFF. <true> }T 
T{ s" MAX-UD" environment? -> FFFFFFFF. <true> }T
decimal

\ ------------------------------------------------------------------------
testing input: accept

create abuf 80 chars allot

: accept-test
   cr ." please type up to 80 characters:" cr
   abuf 80 accept
   dup 
   cr ." received: " [char] " emit
   abuf swap type [char] " emit cr
;

\ The text for accept (below the test) is 29 characters long.
T{ accept-test -> 29 }T
Here is some text for accept.

\ ------------------------------------------------------------------------
testing dictionary search rules

T{ : gdx   123 ; : gdx   gdx 234 ; -> }T
T{ gdx -> 123 234 }T

hex
\ Free memory used for these tests
core_tests

