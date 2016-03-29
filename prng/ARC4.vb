''
' ARC4
' 
' A vb.net translation of AS3 RC4 by Newbie@devnull
' Original AS3 from:
'         Copyright (c) 2007 Henri Torgemane
' 
' Derived from:
'         The jsbn library, Copyright (c) 2003-2005 Tom Wu
' 
' See LICENSE.txt for full license information.
''
Namespace team.devnull.crypto.prng
    Public Class ARC4
        Private i As Integer = 0
        Private j As Integer = 0
        Private Const psize As UInteger = 256
        Private S As Byte()
        Public Sub New(Optional ByRef key As Byte() = Nothing)
            ReDim S(255)
            If (key IsNot Nothing) Then init(key)
        End Sub
        Public Function getPoolSize() As UInteger
            Return psize
        End Function
        Public Sub init(ByRef key As Byte())
            Dim i As Integer
            Dim j As Integer
            Dim t As Integer
            For i = 0 To 255 : S(i) = i : Next
            j = 0
            For i = 0 To 255
                j = (j + S(i) + key(i Mod key.Length)) And 255
                t = S(i)
                S(i) = S(j)
                S(j) = t
            Next
            Me.i = 0
            Me.j = 0
        End Sub
        Public Function fnext() As UInteger
            Dim t As Integer
            i = (i + 1) And 255
            j = (j + S(i)) And 255
            t = S(i)
            S(i) = S(j)
            S(j) = t
            Return S((t + S(i)) And 255)
        End Function
        Public Function getBlockSize() As UInteger
            Return 1
        End Function
        Public Sub encrypt(ByRef block As Byte())
            Dim i As UInteger = 0
            While (i < block.Length)
                block(i) = block(i) Xor fnext()
                i += 1
            End While
        End Sub
        Public Sub decrypt(ByRef block As Byte())
            encrypt(block) ' the beauty Of Xor.
        End Sub
        Public Sub dispose()
            Dim i As UInteger = 0
            Dim r As Random = New Random()
            If (S IsNot Nothing) Then
                For i = 0 To S.Length : S(i) = r.Next(0, 256) : Next
                S = {}
            End If
            Me.i = 0
            Me.j = 0
        End Sub
        Public Overrides Function toString() As String
            Return "rc4"
        End Function
    End Class
End Namespace
