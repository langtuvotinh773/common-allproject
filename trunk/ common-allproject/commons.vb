'------------------- Change string pageing SQL 2012
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
'-----------------------------------------------------------
	Public Function Paging(ByVal stbSql As String, ByVal Page As Integer, Optional ByVal RecordNumber As Integer = 20) As String

        'Paging

        Dim strTemp As String = ""
        stbSql += "  "
        With stbSql
            Dim indexFrom As Integer = .ToString().ToUpper().IndexOf(" FROM ")
            Dim indexWhere As Integer = .ToString().ToUpper().IndexOf(" WHERE ")
            Dim indexOrder As Integer = .ToString().ToUpper().IndexOf(" ORDER ")

            If indexOrder = -1 Then
                indexOrder = .ToString().Length - 1
            End If
            If indexWhere = -1 Then
                indexWhere = indexOrder
            End If

            Dim strSelect As String = ""
            Dim strFrom As String = ""
            Dim strWhere As String = ""
            Dim strOrderBy As String = ""

            strSelect = .ToString().ToUpper().Substring(0, indexFrom).Trim()
            strFrom = .ToString().ToUpper().Substring(indexFrom, indexWhere - indexFrom).Trim()
            strWhere = .ToString().ToUpper().Substring(indexWhere, indexOrder - indexWhere).Trim()
            strOrderBy = .ToString().ToUpper().Substring(indexOrder, .ToString().Length - indexOrder).Replace("ORDER ", " ").Replace("BY ", " ").Trim()

            Dim hasOrder As Boolean = False
            Dim oldOrder As String = ""

            strTemp = "declare @intRow int select @intRow=(Count(*)) " + strFrom + "  " + strWhere + " SELECT @intRow as ROWNUMBER ,* FROM ("

            If String.IsNullOrEmpty(strOrderBy) Then
                If .ToString().ToUpper().IndexOf(" DISTINCT ") > -1 Then
                    strTemp += strSelect.Replace("SELECT ", " ").Replace(" DISTINCT ", "SELECT DISTINCT ROW_NUMBER() OVER ( ORDER BY ( SELECT 1)) as RowIndex , ") + "  " + strFrom + "  " + strWhere
                Else
                    strTemp += strSelect.Replace("SELECT ", "SELECT ROW_NUMBER() OVER ( ORDER BY ( SELECT 1)) as RowIndex , ") + "  " + strFrom + "  " + strWhere
                End If
            Else

                Dim arrOrder As String() = strOrderBy.Split(",".ToCharArray())
                Dim tmpOrder As String = strOrderBy
                If arrOrder.Length > 1 Then
                    tmpOrder = arrOrder(0)
                    hasOrder = True
                    oldOrder = String.Join(",", arrOrder.ToList().Skip(1).ToArray()).Trim()
                End If

                If .ToString().ToUpper().IndexOf(" DISTINCT ") > -1 Then
                    strTemp += strSelect.Replace("SELECT ", " ").Replace(" DISTINCT ", "SELECT DISTINCT ROW_NUMBER() OVER ( ORDER BY ( " + tmpOrder + ")) as RowIndex , ") + "  " + strFrom + "  " + strWhere
                Else
                    strTemp += strSelect.Replace("SELECT ", "SELECT ROW_NUMBER() OVER ( ORDER BY (  " + tmpOrder + ")) as RowIndex , ") + "  " + strFrom + "  " + strWhere
                End If
            End If


            If Page >= 1 Then
                strTemp += " ) B where RowIndex>" + (Page * RecordNumber + 1).ToString() + "and RowIndex<=" + ((Page + 1) * RecordNumber).ToString()
            Else
                strTemp += " ) B where RowIndex> 0 and RowIndex<=" + RecordNumber.ToString()

            End If

            If hasOrder Then
                strTemp += " ORDER BY " + oldOrder
            End If

        End With
        Return strTemp
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