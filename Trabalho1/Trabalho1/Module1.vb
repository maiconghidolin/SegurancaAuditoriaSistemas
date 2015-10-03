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

            Dim inicio = Timer
            Dim chaveTransp = VerificarChaveCifraTransposicao(ArquivosNormais(i), ArquivosCrip(i))
            If chaveTransp <> -1 Then
                Console.WriteLine("Chave: " & chaveTransp)
                Continue For
            End If
            Dim fim = Timer
            Console.WriteLine("Verificar chave de transposicao demorou " & fim - inicio & " segundos")


            Dim ChaveVigenere = VerificarChaveCifraVigenere(ArquivosNormais(i), ArquivosCrip(i))
            If ChaveVigenere <> "" Then
                Console.WriteLine("Chave: " & ChaveVigenere)
                Continue For
            End If

            Console.WriteLine("Chave não encontrada!")
        Next
    End Sub

    Sub SomenteComArquivoCifrado()
        Console.WriteLine("Digite o diretorio do arquivo das palavras!")
        Dim CaminhoPalavras As String = Console.ReadLine()
        Dim ArquivoPalavras = LerArquivo(CaminhoPalavras).Replace(vbCrLf, " ").Split(" ")
        Dim Palavras As HashSet(Of String) = New HashSet(Of String)(ArquivoPalavras)

        Dim continua = True
        While continua
            Console.WriteLine("Digite o caminho do arquivo criptografado!")
            Dim CaminhoCifrado As String = Console.ReadLine()
            Dim arquivoCriptografado = File.ReadAllBytes(CaminhoCifrado)

            Dim Tipo As TipoCifra = 0
            Console.WriteLine("Escolha o tipo da cifra:")
            Console.WriteLine("1 - Cifra de Céasar;")
            Console.WriteLine("2 - Cifra de Transposição;")
            Console.WriteLine("3 - Cifra de Vigenère;")
            Console.WriteLine("4 - Substituição;")
            Tipo = CInt(Console.ReadLine())

            If Tipo = TipoCifra.Ceasar Then
                Dim chaveCeasar = VerificarChaveCifraCeasarForcaBruta(arquivoCriptografado, Palavras)
                If chaveCeasar <> -1 Then
                    Console.WriteLine("Chave: " & chaveCeasar)
                Else
                    Console.WriteLine("Chave não encontrada!")
                End If
            ElseIf Tipo = TipoCifra.Vigenere Then
                Dim chaveVigenere = VerificarChaveCifraVigenereForcaBruta(arquivoCriptografado, Palavras)
                If chaveVigenere IsNot Nothing Then
                    Dim ch = (From obj In chaveVigenere Select Chr(obj)).ToArray
                    Console.WriteLine("Chave: " & ch)
                Else
                    Console.WriteLine("Chave não encontrada!")
                End If
            ElseIf Tipo = TipoCifra.Transposicao Then
                Dim chaveTransposicao = VerificarChaveCifraTransposicaoForcaBruta(arquivoCriptografado, Palavras)
                If chaveTransposicao <> -1 Then
                    Console.WriteLine("Chave: " & chaveTransposicao)
                Else
                    Console.WriteLine("Chave não encontrada!")
                End If
            ElseIf Tipo = TipoCifra.Substituicao Then
                VerificarChaveCifraSubstituicaoForcaBruta(CaminhoCifrado, CaminhoPalavras)
            ElseIf Tipo = 5 Then
                testaGeracaoDeChave()
            End If

            Console.WriteLine("Escolher outro arquivo:")
            Console.WriteLine("1 - Sim;")
            Console.WriteLine("2 - Não;")
            If Console.ReadLine() = 2 Then
                continua = False
            End If
        End While
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

    Function CifraDeCeasarForcaBruta(ByVal Arquivo() As Byte, ByVal Chave As Integer) As HashSet(Of String)
        Dim LinhaCriptografada As New StringBuilder
        For caractere = 3 To Arquivo.Count - 1 ' comeca no 3 por causa dos 3 caracteres estranhos no começo do arquivo
            Dim CaractereAsc As Byte = Arquivo(caractere)
            Dim AscDecifrado = (CaractereAsc - Chave) Mod 256
            If AscDecifrado < 0 Then
                AscDecifrado += 256
            End If
            LinhaCriptografada.Append(Chr(AscDecifrado))
        Next

        Dim qq = LinhaCriptografada.ToString.Replace(vbCrLf, " ").Split(" ")
        Return New HashSet(Of String)(qq)
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

    Function CifraDeTransposicaoForcaBruta(ByVal Arquivo() As Byte, ByVal Chave As Integer) As HashSet(Of String)
        Chave = Arquivo.Count / Chave
        Dim matriz(Chave - 1) As StringBuilder
        For caractere = 0 To Arquivo.Count - 1
            Dim index = caractere Mod Chave
            If matriz(index) Is Nothing Then
                matriz(index) = New StringBuilder
            End If
            matriz(index).Append(Chr(Arquivo(caractere)))
        Next
        Dim LinhaCriptografada As New StringBuilder
        For Each j In matriz
            LinhaCriptografada.Append(j)
        Next
        Dim qq = LinhaCriptografada.ToString.Trim().Replace(vbCrLf, " ").Split(" ")
        Return New HashSet(Of String)(qq)
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

    Function CifraDeVigenereForcaBruta(ByVal Arquivo() As Byte, ByVal Chave As List(Of Byte)) As HashSet(Of String)
        Dim LinhaCriptografada As New StringBuilder
        For caractere = 3 To Arquivo.Count - 1 ' comeca no 3 por causa dos 3 caracteres estranhos no começo do arquivo
            Dim CaractereAsc As Integer = Arquivo(caractere)
            Dim AscDecifrado = (CaractereAsc - Chave(caractere Mod Chave.Count)) Mod 256
            If AscDecifrado < 0 Then
                AscDecifrado += 256
            End If
            LinhaCriptografada.Append(Chr(AscDecifrado))
        Next
        Dim qq = LinhaCriptografada.ToString.Replace(vbCrLf, " ").Split(" ")
        Return New HashSet(Of String)(qq)
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

    Function CifraDeSubstituicaoArquivoBytes(ByVal Arquivo() As Byte, ByVal AlfabetoNormal As List(Of Byte), ByVal AlfabetoCifrado As List(Of Byte), ByVal Acao As Acao, Optional ByVal QtdCaracteres As Integer = -1) As List(Of Byte)
        Dim LinhaCriptografada As New List(Of Byte)
        If QtdCaracteres = -1 Then
            QtdCaracteres = Arquivo.Count - 1
        End If
        If Acao = Trabalho1.Acao.Cifrar Then
            Dim cloneAlfabeto = (From obj In AlfabetoNormal Select obj).ToList()
            Dim rand As New Random()
            For Each caractere In Arquivo
                Dim index = AlfabetoNormal.IndexOf(caractere)
                LinhaCriptografada.Add(AlfabetoCifrado(index))
            Next
        Else
            For caractere = 0 To QtdCaracteres
                Dim index = AlfabetoCifrado.IndexOf(Arquivo(caractere))
                'se -1, significa que n existe no alfabeto cifrado
                'portanto nao existe no alfabeto normal
                'entao se nao existe continua o mesmo caractere do cifrado
                If index = -1 Then
                    LinhaCriptografada.Add(Arquivo(caractere))
                Else
                    LinhaCriptografada.Add(AlfabetoNormal(index))
                End If
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
            Dim inicio = Timer
            Dim novaChave As Integer = arquivoCriptografado.Count / i
            Dim retorno = CifraDeTransposicaoArquivoBytes(arquivoCriptografado, novaChave)
            Dim fim = Timer
            Console.WriteLine("Chave " & i & " demorou " & fim - inicio & " segundos")
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

    Function VerificarChaveCifraCeasarForcaBruta(ByVal Arquivo() As Byte, ByVal Palavras As HashSet(Of String)) As Integer
        For chave = 0 To 255
            Dim retorno = CifraDeCeasarForcaBruta(Arquivo, chave)
            If retorno.IsSubsetOf(Palavras) Then
                Return chave
            End If
        Next
        Return -1
    End Function

    Function VerificarChaveCifraTransposicaoForcaBruta(ByVal Arquivo() As Byte, ByVal Palavras As HashSet(Of String)) As Integer
        For chave = 1 To Arquivo.Count
            Dim retorno = CifraDeTransposicaoForcaBruta(Arquivo, chave)
            If retorno.IsSubsetOf(Palavras) Then
                Return chave
            End If
        Next
        Return -1
    End Function

    Function VerificarChaveCifraVigenereForcaBruta(ByVal Arquivo() As Byte, ByVal Palavras As HashSet(Of String)) As List(Of Byte)

        Console.WriteLine("Descifrar com:")
        Console.WriteLine("1 - Letras;")
        Console.WriteLine("2 - Números;")
        Dim DecifrarCom = Console.ReadLine()

        Dim inicio As Integer
        Dim fim As Integer
        If DecifrarCom = 1 Then
            inicio = 97
            fim = 122
        Else
            inicio = 48
            fim = 57
        End If

        Dim chave As New List(Of Byte)
        chave.Add(inicio)

        While chave IsNot Nothing
            Dim retorno = CifraDeVigenereForcaBruta(Arquivo, chave)
            If retorno.IsSubsetOf(Palavras) Then
                Return chave
            End If
            chave = GeraProximaChaveVigenere(chave, inicio, fim)
        End While

        Return Nothing
    End Function

    Sub VerificarChaveCifraSubstituicaoForcaBruta(ByVal CaminhoCriptografado As String, ByVal CaminhoPalavras As String)

        Dim TriplasCifradas As New Dictionary(Of StringBuilder, Integer)
        Dim TriplasDicionario As New Dictionary(Of StringBuilder, Integer)
        Dim Alfabeto As New Dictionary(Of Char, StringBuilder)
        Dim arquivoCifrado = LerArquivo(CaminhoCriptografado)
        Dim arquivoDicionario = LerArquivo(CaminhoPalavras)

        GeraDicionarioFrequencias(arquivoCifrado, TriplasCifradas)
        GeraDicionarioFrequencias(arquivoDicionario, TriplasDicionario)
        Dim TriplasCifradasOrdenadas = (From item In TriplasCifradas Order By item.Value Descending Select item.Key).ToList
        Dim TriplasDicionarioOrdenadas = (From item In TriplasDicionario Order By item.Value Descending Select item.Key).ToList

        Console.WriteLine("Digite a tripla de início:")
        Dim inicio As Integer = Console.ReadLine()
        Console.WriteLine("Digite a tripla de fim:")
        Dim fim As Integer = Console.ReadLine()

        If fim > TriplasCifradasOrdenadas.Count - 1 Then
            fim = TriplasCifradasOrdenadas.Count - 1
        End If

        If fim > TriplasDicionarioOrdenadas.Count - 1 Then
            fim = TriplasDicionarioOrdenadas.Count - 1
        End If

        For j = inicio To fim
            AdicionarPalavraNoAlfabeto(TriplasCifradasOrdenadas(j).ToString, TriplasDicionarioOrdenadas(j).ToString, Alfabeto)
        Next

        Dim AlfabetoMenorTripla = (From obj In Alfabeto Where obj.Value.Length <= 1 Select obj).ToDictionary(Function(x) x.Key, Function(x) x.Value)

        Alfabeto = New Dictionary(Of Char, StringBuilder)
        Dim palavras() As String = arquivoDicionario.Replace(vbCrLf, " ").Split(" ")
        Dim HashPalavras As HashSet(Of String) = New HashSet(Of String)(palavras)
        'ordena pela palavra de menor tamanho
        'as maiores tem mais chances de serem unicas
        Dim palavrasOrdenadas = (From p In HashPalavras Order By p.Length Descending Select p).ToArray()

        Console.WriteLine("Digite a palavra de início:")
        Dim palavrainicio As Integer = Console.ReadLine()
        Console.WriteLine("Digite a palavra de fim:")
        Dim palavrafim As Integer = Console.ReadLine()

        If palavrafim > palavrasOrdenadas.Count - 1 Then
            palavrafim = palavrasOrdenadas.Count - 1
        End If

        GeraAlfabetoPorPadroes(palavrasOrdenadas, arquivoCifrado, Alfabeto, palavrainicio, palavrafim)

        Dim AlfabetoMenorPadrao = (From obj In Alfabeto Where obj.Value.Length <= 1 Select obj).ToDictionary(Function(x) x.Key, Function(x) x.Value)

        Dim AlfabetoMenorUnido = New Dictionary(Of Char, StringBuilder)
        UneDicionarios(AlfabetoMenorTripla, AlfabetoMenorPadrao, AlfabetoMenorUnido)


        Dim arquivoCriptografado = File.ReadAllBytes(CaminhoCriptografado)
        Dim contador(AlfabetoMenorUnido.Count - 1) As Integer

        While True
            Dim AlfabetoCifrado As New List(Of Byte)
            Dim AlfabetoNormal As New List(Of Byte)

            Dim continua = GeraProximaChaveSubstituicao(AlfabetoCifrado, AlfabetoNormal, AlfabetoMenorUnido, contador)

            Dim retorno = CifraDeSubstituicaoArquivoBytes(arquivoCriptografado, AlfabetoNormal, AlfabetoCifrado, Acao.Descifrar, 20)

            Dim ch = (From qq In retorno Select Chr(qq)).ToArray
            Console.WriteLine(ch)

            If Not continua Then
                Exit While
            End If
        End While

    End Sub

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

    Function GeraProximaChaveVigenere(ByVal Chave As List(Of Byte), ByVal inicio As Integer, ByVal fim As Integer) As List(Of Byte)
        Dim j = Chave.Count - 1
        While j >= 0
            If Chave(j) <> fim Then
                Chave(j) += 1
                Return Chave
            Else
                Chave(j) = inicio
                j -= 1
            End If
        End While
        Chave.Add(inicio)
        Return Chave
    End Function

    Function CalculaPadraoPalavras(ByVal palavra As String) As String
        Dim contador = 0
        Dim hash As New Dictionary(Of Char, Short)
        Dim retorno As String = ""
        For Each obj In palavra
            Dim valor As Short
            If hash.TryGetValue(obj, valor) Then
                retorno &= valor
            Else
                hash.Add(obj, contador)
                retorno &= contador
                contador += 1
            End If
        Next
        Return retorno
    End Function

    Sub AdicionarPalavraNoAlfabeto(ByVal chave As String, ByVal valor As String, ByRef Alfabeto As Dictionary(Of Char, StringBuilder), Optional ByVal unico As Boolean = False)
        For i = 0 To chave.Length - 1
            Dim lista As New StringBuilder
            If Alfabeto.TryGetValue(chave(i), lista) Then
                'se for unica quer dizer que achou a palavra certa
                'entao limpa a lista de possibilidades daquela letra e insere essa unica
                If unico Then
                    lista.Clear()
                    lista.Append(valor(i))
                Else
                    If lista.ToString.IndexOf(valor(i)) = -1 Then
                        lista.Append(valor(i))
                    End If
                End If
            Else
                lista = New StringBuilder
                lista.Append(valor(i))
                Alfabeto.Add(chave(i), lista)
            End If
        Next
    End Sub

    Sub GeraDicionarioFrequencias(ByVal texto As String, ByRef Dicionario As Dictionary(Of StringBuilder, Integer))
        Dim inicio = 0
        Dim fim = inicio + 3
        Dim finaltexto = texto.Count
        While fim <= finaltexto
            Dim tripla As New StringBuilder(texto(inicio) & texto(inicio + 1) & texto(inicio + 2))
            Dim valor As Integer
            If Dicionario.TryGetValue(tripla, valor) Then
                Dicionario(tripla) = valor + 1
            Else
                Dicionario.Add(tripla, 1)
            End If
            inicio += 1
            fim += 1
        End While
    End Sub

    Sub GeraAlfabetoPorPadroes(ByVal palavras() As String, ByVal texto As String, ByRef Alfabeto As Dictionary(Of Char, StringBuilder), ByVal palavraInicio As Integer, ByVal palavraFim As Integer)
        Dim fimtexto = texto.Count
        Dim Frequencia As Dictionary(Of StringBuilder, Integer)
        For obj = palavraInicio To palavraFim
            Dim padrao = CalculaPadraoPalavras(palavras(obj))
            Dim inicio = 0
            Dim fim = inicio + palavras(obj).Length
            Frequencia = New Dictionary(Of StringBuilder, Integer)
            While fim <= fimtexto
                Dim palavra As New StringBuilder()
                For i = inicio To fim - 1
                    palavra.Append(texto(i))
                Next
                Dim padraoCifrado = CalculaPadraoPalavras(palavra.ToString)
                If padrao = padraoCifrado Then
                    Dim valor As Integer
                    If Frequencia.TryGetValue(palavra, valor) Then
                        Frequencia(palavra) = valor + 1
                    Else
                        Frequencia.Add(palavra, 1)
                    End If
                End If
                inicio += 1
                fim += 1
            End While

            If Frequencia.Count = 1 Then
                AdicionarPalavraNoAlfabeto(Frequencia.Keys(0).ToString, palavras(obj), Alfabeto, True)
            Else
                Dim FrequenciasOrdenadas = (From item In Frequencia Order By item.Value Descending Select item.Key).ToList
                If FrequenciasOrdenadas.Count > 0 Then
                    AdicionarPalavraNoAlfabeto(FrequenciasOrdenadas(0).ToString, palavras(obj), Alfabeto, False)
                End If
            End If
        Next
    End Sub

    Function GeraProximaChaveSubstituicao(ByRef AlfabetoCifrado As List(Of Byte), ByRef AlfabetoNormal As List(Of Byte), ByRef Dicionario As Dictionary(Of Char, StringBuilder), ByRef Contador() As Integer) As Boolean
        Dim incrementa = True
        For obj = 0 To Dicionario.Count - 1
            Dim key = Dicionario.Keys.ElementAt(obj)
            AlfabetoCifrado.Add(Asc(key))
            Dim index = Contador(obj)
            Dim value = Dicionario.Values.ElementAt(obj)
            AlfabetoNormal.Add(Asc(value(index)))
            If incrementa Then
                If obj = Dicionario.Count - 1 Then
                    If Contador(obj) = value.ToString.Count - 1 Then
                        Return False
                    End If
                End If

                If Contador(obj) = value.ToString.Count - 1 Then
                    Contador(obj) = 0
                    incrementa = True
                Else
                    Contador(obj) += 1
                    incrementa = False
                End If
            End If
        Next
        Return True
    End Function

    Sub UneDicionarios(ByRef Dicionario1 As Dictionary(Of Char, StringBuilder), ByRef Dicionario2 As Dictionary(Of Char, StringBuilder), ByRef Resultado As Dictionary(Of Char, StringBuilder))
        For Each obj In Dicionario1
            Dim lista As New StringBuilder
            If Resultado.TryGetValue(obj.Key, lista) Then
                If lista.ToString.IndexOf(obj.Value.ToString) = -1 Then
                    lista.Append(obj.Value.ToString)
                End If
            Else
                lista = New StringBuilder
                lista.Append(obj.Value.ToString)
                Resultado.Add(obj.Key, lista)
            End If
        Next

        For Each obj In Dicionario2
            Dim lista As New StringBuilder
            If Resultado.TryGetValue(obj.Key, lista) Then
                If lista.ToString.IndexOf(obj.Value.ToString) = -1 Then
                    lista.Append(obj.Value.ToString)
                End If
            Else
                lista = New StringBuilder
                lista.Append(obj.Value.ToString)
                Resultado.Add(obj.Key, lista)
            End If
        Next
    End Sub

    Sub testaGeracaoDeChave()
        Dim Alfabeto As New Dictionary(Of Char, StringBuilder)
        Alfabeto.Add("a", New StringBuilder("1234"))
        Alfabeto.Add("b", New StringBuilder("12"))
        Alfabeto.Add("c", New StringBuilder("1"))

        Dim contador(Alfabeto.Count - 1) As Integer

        While True
            Dim AlfabetoCifrado As New List(Of Byte)
            Dim AlfabetoNormal As New List(Of Byte)

            Dim continua = GeraProximaChaveSubstituicao(AlfabetoCifrado, AlfabetoNormal, Alfabeto, contador)

            For obj = 0 To AlfabetoCifrado.Count - 1
                Console.Write(Chr(AlfabetoCifrado(obj)) & " - " & Chr(AlfabetoNormal(obj)) & "; ")
            Next
            Console.WriteLine()

            If Not continua Then
                Exit While
            End If

        End While

    End Sub

#End Region

End Module

'sql injection - seminario