Public Shared Function ChangeStringSQLPaging(ByVal stbSql As String, ByVal Page As Integer, Optional ByVal RecordNumber As Integer = 20) As String
        Dim stbNew As New StringBuilder

        With stbSql
            Dim indexFrom As Integer = .ToString().ToUpper().IndexOf(" FROM ")
            Dim indexOrder As Integer = .ToString().ToUpper().IndexOf(" ORDER ")
            If indexOrder = -1 Then
                indexOrder = .ToString().Length - 1
            End If

            Dim strTemp As String = "declare @intRow int select @intRow=(Count(*)) " + .ToString().ToUpper().Substring(indexFrom, indexOrder - indexFrom) + "  "
            stbNew.Append(strTemp)
            If .ToString().ToUpper().IndexOf(" DISTINCT ") > -1 Then
                stbNew.Append(.ToString().ToUpper().Replace("SELECT ", " ").Replace(" DISTINCT ", "SELECT DISTINCT @intRow as ROWNUMBER , "))
            Else
                stbNew.Append(.ToString().ToUpper().Replace("SELECT ", "SELECT @intRow as ROWNUMBER , "))
            End If

            If Page > 1 Then
                stbNew.Append(" OFFSET " + ((Page - 1) * RecordNumber).ToString() + "  ROWS FETCH NEXT " + RecordNumber.ToString() + " ROWS ONLY ")
            Else
                stbNew.Append(" OFFSET 0  ROWS FETCH NEXT " + RecordNumber.ToString() + " ROWS ONLY ")
            End If



        End With
        Return stbNew.ToString()

		
    End Function
	'--------------------------
	 Public Shared Function encryptData(ByVal data As String) As Byte()
            Dim md5Hasher As New System.Security.Cryptography.MD5CryptoServiceProvider()
            Dim hashedBytes As Byte()
            Dim encoder As New System.Text.UTF8Encoding()
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data))
            Return hashedBytes
        End Function

        Public Shared Function md5(ByVal data As String) As String
            Return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower()
        End Function
		'-------------------------------------