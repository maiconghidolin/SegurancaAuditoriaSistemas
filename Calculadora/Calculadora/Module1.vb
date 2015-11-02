Module Module1

#Region "Variáveis"

    Private Property MaiorNumero = 100000000 ' 10 ^ 8

#End Region

#Region "Métodos"

    Sub Main()

        Dim A As String
        Dim B As String
        Dim N As String
        Dim ListaA As New List(Of Int64)
        Dim ListaB As New List(Of Int64)
        Dim ListaN As New List(Of Int64)
        Dim Resultado As New List(Of Int64) ' ler de tras p frente

        Dim Operacao As Short

        Console.WriteLine("Digite o A:")
        A = Console.ReadLine()
        Console.WriteLine("Digite o B:")
        B = Console.ReadLine()
        Console.WriteLine("Digite o N:")
        N = Console.ReadLine()

        Console.WriteLine("Digite a operação:")
        Console.WriteLine("1 - Soma")
        Console.WriteLine("2 - Multiplica")
        Console.WriteLine("3 - Divide")
        Operacao = Console.ReadLine()

        CriaLista(A, B, N, ListaA, ListaB, ListaN)

        If Operacao = 1 Then ' Soma
            Soma(ListaA, ListaB, ListaN, Resultado)
            Resultado.Reverse() ' inverte o array
            For Each obj In Resultado
                Console.Write(obj)
            Next
        ElseIf Operacao = 2 Then ' Multiplica
            Multiplica(ListaA, ListaB, ListaN)
        ElseIf Operacao = 3 Then ' Divide
            Divide(ListaA, ListaB, ListaN)
        End If

        Console.ReadLine()
    End Sub

    Sub CriaLista(ByVal A As String, ByVal B As String, ByVal N As String, ByRef ListaA As List(Of Long), ByRef ListaB As List(Of Long), ByRef ListaN As List(Of Long))
        Dim total = A.Count
        While total >= 8
            Dim substr = A.Substring(total - 8, 8)
            ListaA.Add(CInt(substr))
            total -= 8
        End While
        If total > 0 Then
            Dim substr = A.Substring(0, total)
            ListaA.Add(CInt(substr))
        End If

        total = B.Count
        While total >= 8
            Dim substr = B.Substring(total - 8, 8)
            ListaB.Add(CInt(substr))
            total -= 8
        End While
        If total > 0 Then
            Dim substr = B.Substring(0, total)
            ListaB.Add(CInt(substr))
        End If

        total = N.Count
        While total >= 8
            Dim substr = N.Substring(total - 8, 8)
            ListaN.Add(CInt(substr))
            total -= 8
        End While
        If total > 0 Then
            Dim substr = N.Substring(0, total)
            ListaN.Add(CInt(substr))
        End If

    End Sub

    Sub Soma(ByRef ListaA As List(Of Long), ByRef ListaB As List(Of Long), ByRef ListaN As List(Of Long), ByRef Resultado As List(Of Long))
        Dim quociente As Int64 = 0
        Dim resto As Int64 = 0
        For obj As Int64 = 0 To ListaA.Count - 1
            If obj < ListaB.Count Then
                'o que transbordou incrementa no proximo
                ' o maximo que vai transbordar é 1 pois o maior numero possivel é 99.999.999
                Dim aux = ListaA(obj) + ListaB(obj) + quociente
                quociente = Math.Floor(aux / MaiorNumero)
                resto = aux Mod MaiorNumero
                Resultado.Add(resto)
            Else
                ' se lista do b for menor que a do a
                ' aux nunca vai ser maior que o MaiorNumero
                ' só incrementa o estouro e adiciona na lista
                Dim aux = ListaA(obj) + quociente
                quociente = 0
                Resultado.Add(aux)
            End If
        Next
        If quociente > 0 Then ' se a soma da ultima celula do vetor estourar
            Resultado.Add(quociente)
        End If
    End Sub

    Sub Multiplica(ByRef ListaA As List(Of Long), ByRef ListaB As List(Of Long), ByRef ListaN As List(Of Long))
        Dim NumZeros As Integer = 0
        ' multiplica cada casa de B por A
        ' testa se a multiplicação estourou e cria um novo vetor
        ' multiplica a proxima casa
        ' verifica o numero de zeros p por no fim
        ' testa o estouro e cria outro vetor
        ' soma os dois vetores
        ' cria o proximo vetor com a proxima casa de, B verifica o estouro e soma com o vetor da soma anterior
    End Sub

    Sub Divide(ByRef ListaA As List(Of Long), ByRef ListaB As List(Of Long), ByRef ListaN As List(Of Long))

    End Sub

#End Region

End Module
