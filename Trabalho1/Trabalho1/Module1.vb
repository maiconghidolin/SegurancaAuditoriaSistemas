Imports System
Imports System.IO
Imports System.Collections

#Region "Enumeradores"

Enum TipoCifra
    Ceasar = 1
    Transposicao = 2
    Vigenere = 3
    Substituicao = 4
End Enum

Enum Acao
    Cifrar = 1
    Descifrar = 2
End Enum

#End Region

Module Module1

    Sub Main()
        Dim Tipo As TipoCifra = 0
        While Tipo <= 0 Or Tipo > 4
            Console.WriteLine("Escolha o tipo da cifra:")
            Console.WriteLine("1 - Cifra de Céasar;")
            Console.WriteLine("2 - Cifra de Transposição;")
            Console.WriteLine("3 - Cifra de Vigenère;")
            Console.WriteLine("4 - Cifra de Substituição;")
            Tipo = CInt(Console.ReadLine())
            If Tipo <= 0 Or Tipo > 4 Then
                Console.WriteLine("Escolha inválida!")
            End If
        End While

        Dim Acao As Acao = 0
        While Acao <= 0 Or Acao > 2
            Console.WriteLine("Escolha a ação:")
            Console.WriteLine("1 - Cifrar;")
            Console.WriteLine("2 - Descifrar;")
            Acao = CInt(Console.ReadLine())
            If Acao <= 0 Or Acao > 2 Then
                Console.WriteLine("Escolha inválida!")
            End If
        End While

        If Acao = Trabalho1.Acao.Cifrar Then
            Console.WriteLine("Digite o caminho do texto a ser criptografado!")
        Else
            Console.WriteLine("Digite o caminho do texto já criptografado!")
        End If
        Dim Caminho = Console.ReadLine()

        If Tipo = TipoCifra.Ceasar Or Tipo = TipoCifra.Transposicao Then
            Console.WriteLine("Digite a chave(número)!")
            Dim Chave = CInt(Console.ReadLine())

            If Tipo = TipoCifra.Ceasar Then
                CifraDeCeasar(Caminho, Chave, Acao)
            End If

            If Tipo = TipoCifra.Transposicao Then
                CifraDeTransposicao(Caminho, Chave, Acao)
            End If

        ElseIf Tipo = TipoCifra.Vigenere Then
            Console.WriteLine("Digite a chave(palavra)!")
            Dim Chave = Console.ReadLine()
            CifraDeVigenere(Caminho, Chave, Acao)
        ElseIf Tipo = TipoCifra.Substituicao Then
            CifraDeSubstituicao(Caminho, Acao)
        End If

        Console.ReadLine()
    End Sub

    Sub CifraDeCeasar(ByVal Caminho As String, ByVal Chave As Integer, ByVal Acao As Acao)
        Dim Texto = LerArquivo(Caminho)
        Dim LinhaCriptografada As String = ""
        For Each caractere In Texto
            Dim CaractereAsc As Integer = Asc(caractere) ' Converte para integer.
            Dim CaractereChar As Char
            If Acao = Trabalho1.Acao.Cifrar Then
                CaractereChar = Chr(((CaractereAsc + Chave) Mod 256)) ' Converte para char.
            ElseIf Acao = Trabalho1.Acao.Descifrar Then
                CaractereChar = Chr(((CaractereAsc - Chave) Mod 256)) ' Converte para char.
            End If
            LinhaCriptografada &= CaractereChar
        Next
        Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
        Dim CaminhoGerado = Console.ReadLine()
        EscreverArquivo(LinhaCriptografada, CaminhoGerado)
    End Sub

    Sub CifraDeTransposicao(ByVal Caminho As String, ByVal Chave As Integer, ByVal Acao As Acao)
        Dim texto = LerArquivo(Caminho)
        If Acao = Trabalho1.Acao.Descifrar Then
            Chave = texto.Count / Chave
        End If
        Dim LinhaCriptografada As String = ""
        Dim i As Integer = -1
        Dim matriz(Chave) As String
        For Each caractere In texto
            i += 1
            If i > Chave - 1 Then
                i = 0
            End If
            matriz(i) &= caractere
        Next
        For j As Integer = i + 1 To Chave - 1
            matriz(j) &= " "
        Next
        For Each j In matriz
            LinhaCriptografada += j
        Next
        Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
        Dim CaminhoGerado = Console.ReadLine()
        EscreverArquivo(LinhaCriptografada, CaminhoGerado)
    End Sub

    Sub CifraDeVigenere(ByVal Caminho As String, ByVal Chave As String, ByVal Acao As Acao)
        Dim texto = LerArquivo(Caminho)
        Dim chaveInt() As Integer = (From obj In Chave Select Asc(obj)).ToArray
        Dim LinhaCriptografada As String = ""
        Dim i As Integer = -1
        For Each caractere In texto
            i += 1
            If i > chaveInt.Count - 1 Then
                i = 0
            End If
            Dim CaractereAsc As Integer = Asc(caractere)
            Dim CaractereChar As Char
            If Acao = Trabalho1.Acao.Cifrar Then
                CaractereChar = Chr(((CaractereAsc + chaveInt(i)) Mod 256))
            ElseIf Acao = Trabalho1.Acao.Descifrar Then
                CaractereChar = Chr(((CaractereAsc - chaveInt(i)) Mod 256))
            End If
            LinhaCriptografada &= CaractereChar
        Next
        Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
        Dim CaminhoGerado = Console.ReadLine()
        EscreverArquivo(LinhaCriptografada, CaminhoGerado)
    End Sub

    Sub CifraDeSubstituicao(ByVal Caminho As String, ByVal Acao As Acao)
        Dim texto = LerArquivo(Caminho)
        Dim Alfabeto = CriaAlfabeto()
        Dim LinhaCriptografada As String = ""
        If Acao = Trabalho1.Acao.Cifrar Then
            Dim cloneAlfabeto = (From obj In Alfabeto Select obj).tolist()
            Dim NovoAlfabeto As String = ""
            Dim rand As New Random()
            For i = 0 To Alfabeto.Count - 1
                Dim Index As Integer = rand.Next(cloneAlfabeto.Count)
                NovoAlfabeto &= Chr(cloneAlfabeto(Index))
                cloneAlfabeto.RemoveAt(Index)
            Next
            For Each caractere In texto
                Dim index = Alfabeto.IndexOf(Asc(caractere))
                LinhaCriptografada &= NovoAlfabeto(index)
            Next
            Console.WriteLine("Digite o caminho para escrever o alfabeto substituido!")
            Caminho = Console.ReadLine()
            EscreverArquivo(NovoAlfabeto, Caminho)
        Else
            Console.WriteLine("Digite o caminho do alfabeto substituido!")
            Caminho = Console.ReadLine()
            Dim AlfabetoSubstituido = LerArquivo(Caminho)
            For Each caractere In texto
                Dim index = AlfabetoSubstituido.IndexOf(caractere)
                LinhaCriptografada &= Chr(Alfabeto(index))
            Next
        End If
        Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
        Caminho = Console.ReadLine()
        EscreverArquivo(LinhaCriptografada, Caminho)
    End Sub

#Region "Manipuladores de Arquivos"

    Function LerArquivo(ByVal caminho As String) As String
        Dim Stream As New StreamReader(caminho)
        Dim texto = Stream.ReadToEnd()
        Stream.Close()
        Return texto
    End Function

    Sub EscreverArquivo(ByVal texto As String, ByVal caminho As String)
        Dim Stream As New StreamWriter(caminho)
        Stream.Write(texto)
        Stream.Close()
        Console.WriteLine("Arquivo gerado em " & caminho & ";")
    End Sub

#End Region

#Region "Outras Funções"

    Function CriaAlfabeto() As List(Of Integer)
        Dim Alfabeto As New List(Of Integer)
        For i = 0 To 255
            Alfabeto.Add(i)
        Next
        Return Alfabeto
    End Function

#End Region

End Module

'decrifar ceasar: pega o primeiro do cifrado - primeiro do normal

'decifra trasnposição por força bruta..tenta todas as chaves possíveis

'decifra  vigenere: le posição por posição diminuindo o cifrado do normal

'decifra substituição: pega posição por posição, compara e monta a chave. primeira posição do normal é 'a' e a primeira do cifrado é 'z', então 'a' corresponde a 'z'.