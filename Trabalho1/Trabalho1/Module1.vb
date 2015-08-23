Imports System
Imports System.IO
Imports System.Collections
Imports System.Text
Imports System.Linq

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

Enum TipoArquivo
    Normal = 1
    NormalECifrado = 2
    Cifrado = 3
End Enum

#End Region

Module Module1

    Sub Main()

        Dim TipoArquivo As TipoArquivo = 0
        While TipoArquivo <= 0 Or TipoArquivo > 3
            Console.WriteLine("Quais arquivos voçê possui:")
            Console.WriteLine("1 - Somente o Normal;")
            Console.WriteLine("2 - Normal e Cifrado;")
            Console.WriteLine("3 - Somente o Cifrado;")
            TipoArquivo = CInt(Console.ReadLine())
            If TipoArquivo <= 0 Or TipoArquivo > 3 Then
                Console.WriteLine("Escolha inválida!")
            End If
        End While

        If TipoArquivo = Trabalho1.TipoArquivo.Normal Then
            SomenteComArquivoNormal()
        ElseIf TipoArquivo = Trabalho1.TipoArquivo.NormalECifrado Then
            ComArquivoNormalECifrado()
        ElseIf TipoArquivo = Trabalho1.TipoArquivo.Cifrado Then
            SomenteComArquivoCifrado()
        End If

        Console.ReadLine()
    End Sub

    Sub SomenteComArquivoNormal()
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
    End Sub

    Sub ComArquivoNormalECifrado()
        Console.WriteLine("Digite o diretorio dos arquivos normais!")
        Dim CaminhoNormal As String = Console.ReadLine()
        Console.WriteLine("Digite o diretorio dos arquivos criptografados!")
        Dim CaminhoCifrado As String = Console.ReadLine()

        Dim ArquivosNormais = Directory.GetFiles(CaminhoNormal)
        Dim ArquivosCrip = Directory.GetFiles(CaminhoCifrado)

        For i = 0 To ArquivosNormais.Count - 1

            Dim ChaveCeasar = VerificarChaveCifraCeasar(ArquivosNormais(i), ArquivosCrip(i))
            If ChaveCeasar <> -1 Then
                Console.WriteLine("Chave: " & ChaveCeasar)
                Continue For
            End If

            Dim ChaveSubstituicao = VerificarChaveCifraSubstituicao(ArquivosNormais(i), ArquivosCrip(i))
            If ChaveSubstituicao <> "" Then
                Console.WriteLine("Chave: " & ChaveSubstituicao)
                Continue For
            End If

            Dim chaveTransp = VerificarChaveCifraTransposicao(ArquivosNormais(i), ArquivosCrip(i))
            If chaveTransp <> -1 Then
                Console.WriteLine("Chave: " & chaveTransp)
                Continue For
            End If

            Dim ChaveVigenere = VerificarChaveCifraVigenere(ArquivosNormais(i), ArquivosCrip(i))
            If ChaveVigenere <> "" Then
                Console.WriteLine("Chave: " & ChaveVigenere)
                Continue For
            End If

            Console.WriteLine("Chave não encontrada!")
        Next
    End Sub

    Sub SomenteComArquivoCifrado()

    End Sub

#Region "Funções Para Cifrar e Descifrar"

    Function CifraDeCeasar(ByVal Caminho As String, ByVal Chave As Integer, ByVal Acao As Acao, Optional ByVal EscreveArquivo As Boolean = True) As String
        Dim Texto = LerArquivo(Caminho)
        Dim LinhaCriptografada As New StringBuilder
        For caractere = 0 To Texto.Count - 1
            Dim CaractereAsc As Integer = Asc(Texto(caractere)) ' Converte para integer.
            Dim CaractereChar As Char
            If Acao = Trabalho1.Acao.Cifrar Then
                CaractereChar = Chr(((CaractereAsc + Chave) Mod 256)) ' Converte para char.
            ElseIf Acao = Trabalho1.Acao.Descifrar Then
                Dim AscDecifrado = (CaractereAsc - Chave) Mod 256
                If AscDecifrado < 0 Then
                    AscDecifrado += 256
                End If
                CaractereChar = Chr(AscDecifrado) ' Converte para char.
            End If
            LinhaCriptografada.Append(CaractereChar)
        Next
        If EscreveArquivo Then
            Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
            Dim CaminhoGerado = Console.ReadLine()
            EscreverArquivo(LinhaCriptografada.ToString, CaminhoGerado)
            Return ""
        Else
            Return LinhaCriptografada.ToString
        End If
    End Function

    Function CifraDeCeasarArquivoBytes(ByVal Arquivo() As Byte, ByVal Chave As Integer, ByVal Acao As Acao) As List(Of Byte)
        Dim LinhaCriptografada As New List(Of Byte)
        For caractere = 0 To Arquivo.Count - 1
            Dim CaractereAsc As Byte = Arquivo(caractere)
            Dim CaractereChar As Byte
            If Acao = Trabalho1.Acao.Cifrar Then
                CaractereChar = (CaractereAsc + Chave) Mod 256
            ElseIf Acao = Trabalho1.Acao.Descifrar Then
                Dim AscDecifrado = (CaractereAsc - Chave) Mod 256
                If AscDecifrado < 0 Then
                    AscDecifrado += 256
                End If
                CaractereChar = AscDecifrado
            End If
            LinhaCriptografada.Add(CaractereChar)
        Next
        Return LinhaCriptografada
    End Function

    Function CifraDeTransposicao(ByVal Caminho As String, ByVal Chave As Integer, ByVal Acao As Acao, Optional ByVal EscreveArquivo As Boolean = True) As String
        Dim texto = LerArquivo(Caminho)

        If Acao = Trabalho1.Acao.Descifrar Then
            Chave = texto.Count / Chave
        End If

        Dim LinhaCriptografada As New StringBuilder
        Dim i As Integer = -1
        Dim matriz(Chave) As StringBuilder
        For Each caractere In texto
            i += 1
            If i > Chave - 1 Then
                i = 0
            End If
            If matriz(i) Is Nothing Then
                matriz(i) = New StringBuilder
            End If
            matriz(i).Append(caractere)
        Next

        For j As Integer = i + 1 To Chave - 1
            matriz(j).Append(" ")
        Next

        For Each j In matriz
            LinhaCriptografada.Append(j)
        Next
        If EscreveArquivo Then
            Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
            Dim CaminhoGerado = Console.ReadLine()
            EscreverArquivo(LinhaCriptografada.ToString, CaminhoGerado)
            Return ""
        Else
            Return LinhaCriptografada.ToString
        End If
    End Function

    Function CifraDeTransposicaoArquivoBytes(ByVal Arquivo() As Byte, ByVal Chave As Integer) As List(Of Byte)
        Dim matriz(Chave - 1) As List(Of Byte)
        For caractere = 0 To Arquivo.Count - 1
            Dim index = caractere Mod Chave
            If matriz(index) Is Nothing Then
                matriz(index) = New List(Of Byte)
            End If
            matriz(index).Add(Arquivo(caractere))
        Next
        Dim LinhaCriptografada As New List(Of Byte)
        For Each j In matriz
            LinhaCriptografada.AddRange(j)
        Next
        Return LinhaCriptografada
    End Function

    Function CifraDeVigenere(ByVal Caminho As String, ByVal Chave As String, ByVal Acao As Acao, Optional ByVal EscreveArquivo As Boolean = True) As String
        Dim texto = LerArquivo(Caminho)
        Dim chaveInt() As Integer = (From obj In Chave Select Asc(obj)).ToArray
        Dim LinhaCriptografada As New StringBuilder
        Dim i As Integer = -1
        For caractere = 0 To texto.Count - 1
            i += 1
            If i > chaveInt.Count - 1 Then
                i = 0
            End If
            Dim CaractereAsc As Integer = Asc(texto(caractere))
            Dim CaractereChar As Char
            If Acao = Trabalho1.Acao.Cifrar Then
                CaractereChar = Chr(((CaractereAsc + chaveInt(i)) Mod 256))
            ElseIf Acao = Trabalho1.Acao.Descifrar Then
                Dim AscDecifrado = (CaractereAsc - chaveInt(i)) Mod 256
                If AscDecifrado < 0 Then
                    AscDecifrado += 256
                End If
                CaractereChar = Chr(AscDecifrado)
            End If
            LinhaCriptografada.Append(CaractereChar)
        Next

        If EscreveArquivo Then
            Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
            Dim CaminhoGerado = Console.ReadLine()
            EscreverArquivo(LinhaCriptografada.ToString, CaminhoGerado)
            Return ""
        Else
            Return LinhaCriptografada.ToString
        End If

    End Function

    Function CifraDeVigenereArquivoBytes(ByVal Arquivo() As Byte, ByVal Chave As List(Of Byte), ByVal Acao As Acao) As List(Of Byte)
        Dim LinhaCriptografada As New List(Of Byte)
        Dim i As Integer = -1
        For caractere = 0 To Arquivo.Count - 1
            i += 1
            If i > Chave.Count - 1 Then
                i = 0
            End If
            Dim CaractereAsc As Integer = Arquivo(caractere)
            Dim CaractereChar As Integer
            If Acao = Trabalho1.Acao.Cifrar Then
                CaractereChar = (CaractereAsc + Chave(i)) Mod 256
            ElseIf Acao = Trabalho1.Acao.Descifrar Then
                Dim AscDecifrado = (CaractereAsc - Chave(i)) Mod 256
                If AscDecifrado < 0 Then
                    AscDecifrado += 256
                End If
                CaractereChar = AscDecifrado
            End If
            LinhaCriptografada.Add(CaractereChar)
        Next
        Return LinhaCriptografada
    End Function

    Sub CifraDeSubstituicao(ByVal Caminho As String, ByVal Acao As Acao)
        Dim texto = LerArquivo(Caminho)
        Dim Alfabeto = CriaAlfabeto()
        Dim LinhaCriptografada As New StringBuilder
        If Acao = Trabalho1.Acao.Cifrar Then
            Dim cloneAlfabeto = (From obj In Alfabeto Select obj).ToList()
            Dim NovoAlfabeto As New StringBuilder
            Dim rand As New Random()
            For i = 0 To Alfabeto.Count - 1
                Dim Index As Integer = rand.Next(cloneAlfabeto.Count)
                NovoAlfabeto.Append(Chr(cloneAlfabeto(Index)))
                cloneAlfabeto.RemoveAt(Index)
            Next
            For Each caractere In texto
                Dim index = Alfabeto.IndexOf(Asc(caractere))
                LinhaCriptografada.Append(NovoAlfabeto(index))
            Next
            Console.WriteLine("Digite o caminho para escrever o alfabeto substituido!")
            Caminho = Console.ReadLine()
            EscreverArquivo(NovoAlfabeto.ToString, Caminho)
        Else
            Console.WriteLine("Digite o caminho do alfabeto substituido!")
            Caminho = Console.ReadLine()
            Dim AlfabetoSubstituido = LerArquivo(Caminho)
            For Each caractere In texto
                Dim index = AlfabetoSubstituido.IndexOf(caractere)
                LinhaCriptografada.Append(Chr(Alfabeto(index)))
            Next
        End If
        Console.WriteLine("Digite o caminho para escrever o arquivo gerado!")
        Caminho = Console.ReadLine()
        EscreverArquivo(LinhaCriptografada.ToString, Caminho)
    End Sub

    Function CifraDeSubstituicaoArquivoBytes(ByVal Arquivo() As Byte, ByVal AlfabetoNormal As List(Of Byte), ByVal AlfabetoCifrado As List(Of Byte), ByVal Acao As Acao) As List(Of Byte)
        Dim LinhaCriptografada As New List(Of Byte)
        If Acao = Trabalho1.Acao.Cifrar Then
            Dim cloneAlfabeto = (From obj In AlfabetoNormal Select obj).ToList()
            Dim rand As New Random()
            For Each caractere In Arquivo
                Dim index = AlfabetoNormal.IndexOf(caractere)
                LinhaCriptografada.Add(AlfabetoCifrado(index))
            Next
        Else
            For Each caractere In Arquivo
                Dim index = AlfabetoCifrado.IndexOf(caractere)
                LinhaCriptografada.Add(AlfabetoNormal(index))
            Next
        End If
        Return LinhaCriptografada
    End Function

#End Region

#Region "Funções Para Encontrar Chave"

    Function VerificarChaveCifraCeasar(ByVal CaminhoNormal As String, ByVal CaminhoCriptografado As String) As Integer
        Dim arquivoNormal = File.ReadAllBytes(CaminhoNormal)
        Dim arquivoCriptografado = File.ReadAllBytes(CaminhoCriptografado)

        Dim CaractereAscCriptografado As Integer = arquivoCriptografado(1)
        Dim CaractereAscNormal As Integer = arquivoNormal(1)
        Dim chave As Integer = (CaractereAscCriptografado - CaractereAscNormal) Mod 256
        If chave < 0 Then
            chave += 256
        End If

        Dim retorno = CifraDeCeasarArquivoBytes(arquivoCriptografado, chave, Acao.Descifrar)
        If arquivoNormal.Except(retorno).Count <= 0 Then
            Return chave
        Else
            Return -1
        End If
    End Function

    Function VerificarChaveCifraTransposicao(ByVal CaminhoNormal As String, ByVal CaminhoCriptografado As String) As Integer
        Dim arquivoNormal = File.ReadAllBytes(CaminhoNormal)
        Dim arquivoCriptografado = File.ReadAllBytes(CaminhoCriptografado)

        Dim Chave As Integer = -1
        For i = 1 To arquivoCriptografado.Count
            Dim novaChave As Integer = arquivoCriptografado.Count / i
            Dim retorno = CifraDeTransposicaoArquivoBytes(arquivoCriptografado, novaChave)
            If arquivoNormal.Except(retorno).Count <= 0 Then
                Chave = i
                Exit For
            End If
        Next
        Return Chave
    End Function

    Function VerificarChaveCifraVigenere(ByVal CaminhoNormal As String, ByVal CaminhoCriptografado As String) As String
        Dim arquivoNormal = File.ReadAllBytes(CaminhoNormal)
        Dim arquivoCriptografado = File.ReadAllBytes(CaminhoCriptografado)

        Dim chave As New List(Of Byte)
        For i = 0 To arquivoNormal.Count - 1
            Dim CaractereAscNormal As Integer = arquivoNormal(i)
            Dim CaractereAscCriptografado As Integer = arquivoCriptografado(i)
            Dim AscChave = (CaractereAscCriptografado - CaractereAscNormal) Mod 256
            If AscChave < 0 Then
                AscChave += 256
            End If
            chave.Add(AscChave)
        Next

        Dim retorno = CifraDeVigenereArquivoBytes(arquivoCriptografado, chave, Acao.Descifrar)

        If arquivoNormal.Except(retorno).Count <= 0 Then
            Dim ch = (From obj In chave Take 10 Select Chr(obj)).ToArray
            Return ch
        Else
            Return ""
        End If

    End Function

    Function VerificarChaveCifraSubstituicao(ByVal CaminhoNormal As String, ByVal CaminhoCriptografado As String) As String
        Dim arquivoNormal = File.ReadAllBytes(CaminhoNormal)
        Dim arquivoCriptografado = File.ReadAllBytes(CaminhoCriptografado)

        Dim AlfabetoNormal As New List(Of Byte)
        Dim AlfabetoCifrado As New List(Of Byte)
        For i = 0 To arquivoNormal.Count - 1
            Dim caractereNormal = arquivoNormal(i)
            Dim caractereCifrado = arquivoCriptografado(i)

            Dim indexCifrado = AlfabetoCifrado.IndexOf(caractereCifrado)
            If indexCifrado = -1 Then
                AlfabetoNormal.Add(caractereNormal)
                AlfabetoCifrado.Add(caractereCifrado)
            End If
        Next

        Dim retorno = CifraDeSubstituicaoArquivoBytes(arquivoCriptografado, AlfabetoNormal, AlfabetoCifrado, Acao.Descifrar)

        If arquivoNormal.Except(retorno).Count <= 0 Then
            Dim ch = (From obj In AlfabetoCifrado Take 10 Select Chr(obj)).ToArray
            Return ch
        Else
            Return ""
        End If

    End Function

#End Region

#Region "Manipuladores de Arquivos"

    Function LerArquivo(ByVal caminho As String) As String
        Dim Stream As New StreamReader(caminho, System.Text.Encoding.Default)
        Dim texto = Stream.ReadToEnd()
        Stream.Close()
        Return texto
    End Function

    Sub EscreverArquivo(ByVal texto As String, ByVal caminho As String)
        Dim Stream As New StreamWriter(caminho, False, System.Text.Encoding.Default)
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


'set hash