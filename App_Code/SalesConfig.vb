Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports OfficeOpenXml

Public Class SalesConfig
    Dim myConn As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Public Function GetListData(thisString As String) As DataSet
        Dim thisCmd As New SqlCommand(thisString)
        Using thisConn As New SqlConnection(myConn)
            Using thisAdapter As New SqlDataAdapter()
                thisCmd.Connection = thisConn
                thisAdapter.SelectCommand = thisCmd
                Using thisDataSet As New DataSet()
                    thisAdapter.Fill(thisDataSet)
                    Return thisDataSet
                End Using
            End Using
        End Using
    End Function

    Public Function GetItemData(thisString As String) As String
        Dim result As String = String.Empty
        Using thisConn As New SqlConnection(myConn)
            thisConn.Open()
            Using myCmd As New SqlCommand(thisString, thisConn)
                Using rdResult = myCmd.ExecuteReader
                    While rdResult.Read
                        result = rdResult.Item(0).ToString()
                    End While
                End Using
            End Using
            thisConn.Close()
        End Using
        Return result
    End Function

    Public Function GetItemData_Integer(thisString As String) As Integer
        Dim result As Double = 0
        Try
            Using thisConn As New SqlConnection(myConn)
                thisConn.Open()
                Using myCmd As New SqlCommand(thisString, thisConn)
                    Using rdResult = myCmd.ExecuteReader
                        While rdResult.Read
                            result = rdResult.Item(0)
                        End While
                    End Using
                End Using
                thisConn.Close()
            End Using
        Catch ex As Exception
            result = 0
        End Try
        Return result
    End Function

    Public Function GetItemData_Decimal(thisString As String) As Decimal
        Dim result As Double = 0.00
        Try
            Using thisConn As New SqlConnection(myConn)
                thisConn.Open()
                Using myCmd As New SqlCommand(thisString, thisConn)
                    Using rdResult = myCmd.ExecuteReader
                        While rdResult.Read
                            result = rdResult.Item(0)
                        End While
                    End Using
                End Using
                thisConn.Close()
            End Using
        Catch ex As Exception
            result = 0.00
        End Try
        Return result
    End Function

    Public Sub Log_Sales(data As Object())
        Try
            If data.Length = 4 Then
                Dim type As String = Convert.ToString(data(0))
                Dim dataId As String = Convert.ToString(data(1))
                Dim loginId As String = Convert.ToString(data(2))
                Dim description As String = Convert.ToString(data(3))

                Using thisConn As SqlConnection = New SqlConnection(myConn)
                    Using myCmd As SqlCommand = New SqlCommand("INSERT INTO Log_Sales VALUES (NEWID(), @Type, @DataId, @ActionBy, GETDATE(), @Description)")
                        myCmd.Parameters.AddWithValue("@Type", type)
                        myCmd.Parameters.AddWithValue("@DataId", UCase(dataId).ToString())
                        myCmd.Parameters.AddWithValue("@ActionBy", UCase(loginId).ToString())
                        myCmd.Parameters.AddWithValue("@Description", description)
                        myCmd.Connection = thisConn
                        thisConn.Open()
                        myCmd.ExecuteNonQuery()
                        thisConn.Close()
                    End Using
                End Using
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub GenerateReportRoss(SortData As String, FilePath As String)
        Try
            Using package As New ExcelPackage()
                Dim worksheet = package.Workbook.Worksheets.Add("Sales Data")

                Dim salesHeaders As String() = {"", "Counter Sales", "Budget", "Invoiced", "% To Budget", "Calculated Net Counter Sales", "End of Day Pending Sales ex GST"}
                Dim colIndex As Integer = 1

                For Each header As String In salesHeaders
                    worksheet.Cells(1, colIndex).Value = header
                    worksheet.Cells(1, colIndex).Style.Font.Bold = True
                    worksheet.Cells(1, colIndex).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                    worksheet.Cells(1, colIndex).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                    worksheet.Cells(1, colIndex).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)
                    colIndex += 1
                Next

                worksheet.View.FreezePanes(2, 1)
                Dim startRow As Integer = 2

                Dim startDate As DateTime
                Dim endDate As DateTime

                Dim sort As String = " WHERE YEAR(OrderDate) = YEAR(GETDATE()) AND MONTH(OrderDate) = MONTH(GETDATE())"
                If Not String.IsNullOrEmpty(SortData) Then
                    Dim parts() As String = SortData.Split("-"c)
                    Dim bulan As String = parts(0)
                    Dim tahun As String = parts(1)

                    sort = " WHERE YEAR(OrderDate) = " & tahun & " AND MONTH(OrderDate) = " & bulan
                End If

                Dim thisData As DataSet = GetListData(String.Format("SELECT * FROM Sales {0} ORDER BY OrderDate ASC", sort))
                If thisData.Tables(0).Rows.Count > 0 Then
                    startDate = Convert.ToDateTime(thisData.Tables(0).Rows(0)("OrderDate"))
                    endDate = Convert.ToDateTime(thisData.Tables(0).Rows(thisData.Tables(0).Rows.Count - 1)("OrderDate"))

                    For i As Integer = 0 To thisData.Tables(0).Rows.Count - 1
                        Dim persenToBudget As Decimal = thisData.Tables(0).Rows(0).Item("PersenToBudget") / 100

                        worksheet.Cells(startRow, 1).Value = Convert.ToDateTime(thisData.Tables(0).Rows(0).Item("OrderDate")).ToString("dd MMM yyyy")
                        worksheet.Cells(startRow, 2).Value = thisData.Tables(0).Rows(0).Item("CounterSales")
                        worksheet.Cells(startRow, 3).Value = thisData.Tables(0).Rows(0).Item("Budget")
                        worksheet.Cells(startRow, 4).Value = thisData.Tables(0).Rows(0).Item("Invoiced")
                        worksheet.Cells(startRow, 5).Value = persenToBudget
                        worksheet.Cells(startRow, 6).Value = thisData.Tables(0).Rows(0).Item("CalculatedNet")
                        worksheet.Cells(startRow, 7).Value = thisData.Tables(0).Rows(0).Item("PendingSales")

                        worksheet.Cells(startRow, 2).Style.Numberformat.Format = "$#,##0.00"
                        worksheet.Cells(startRow, 3).Style.Numberformat.Format = "$#,##0.00"
                        worksheet.Cells(startRow, 4).Style.Numberformat.Format = "$#,##0.00"
                        worksheet.Cells(startRow, 5).Style.Numberformat.Format = "0.00%"
                        worksheet.Cells(startRow, 6).Style.Numberformat.Format = "$#,##0.00"
                        worksheet.Cells(startRow, 7).Style.Numberformat.Format = "$#,##0.00"

                        startRow += 1
                    Next

                    Dim productionStartRow As Integer = startRow + 2
                    worksheet.Cells(productionStartRow, 1).Value = "Product"
                    worksheet.Cells(productionStartRow, 2).Value = "Production In"
                    worksheet.Cells(productionStartRow, 3).Value = "Production Out"

                    For col As Integer = 1 To 3
                        worksheet.Cells(productionStartRow, col).Style.Font.Bold = True
                        worksheet.Cells(productionStartRow, col).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                        worksheet.Cells(productionStartRow, col).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        worksheet.Cells(productionStartRow, col).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White)
                    Next

                    Dim productNames As String() = {"PVC Shutters Huake", "PVC Shutters Evolve", "Roller O", "Roller J", "Roller S", "Vertical O", "Vertical J", "Panel Glide O", "Panel Glide J", "Venetian O", "Venetian J", "Pelmet O", "Pelmet J", "Zebra", "Curtain", "Cellular Shades", "Veri Shades"}

                    Dim prodRow As Integer = productionStartRow + 1
                    For Each productName As String In productNames
                        Dim productionIn As Integer = GetProductionIn(productName, startDate, endDate)
                        Dim productionOut As Integer = GetProductionOut(productName, startDate, endDate)

                        worksheet.Cells(prodRow, 1).Value = productName ' Product
                        worksheet.Cells(prodRow, 2).Value = productionIn ' Production In
                        worksheet.Cells(prodRow, 3).Value = productionOut ' Production Out
                        prodRow += 1
                    Next

                    worksheet.Cells(productionStartRow + 1, 2, prodRow - 1, 3).Style.Numberformat.Format = "#,##0"

                    worksheet.Cells(1, 1, startRow - 1, 7).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
                    worksheet.Cells(1, 1, startRow - 1, 7).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
                    worksheet.Cells(1, 1, startRow - 1, 7).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                    worksheet.Cells(1, 1, startRow - 1, 7).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin

                    worksheet.Cells(productionStartRow, 1, prodRow - 1, 3).Style.Border.Top.Style = Style.ExcelBorderStyle.Thin
                    worksheet.Cells(productionStartRow, 1, prodRow - 1, 3).Style.Border.Left.Style = Style.ExcelBorderStyle.Thin
                    worksheet.Cells(productionStartRow, 1, prodRow - 1, 3).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                    worksheet.Cells(productionStartRow, 1, prodRow - 1, 3).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin

                    worksheet.Cells.AutoFitColumns()

                    package.SaveAs(New FileInfo(FilePath))
                End If
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Function GetProductionIn(Data As String, Start As Date, Last As Date) As Integer
        Dim result As Integer = 0
        Try
            If Not Data = "" Then
                Dim designId As String = String.Empty
                Dim production As String = String.Empty

                If Data = "PVC Shutters Huake" Then
                    designId = "0CB7C37F-D478-49BA-94CB-DCDE83FB84C8"
                    production = "Orion"
                End If

                If Data = "Shutters Evolve" Then
                    designId = "F70CD0D8-06E5-4C99-B8D8-E9506C1A0F12" : production = "Orion"
                End If

                If Data = "Roller O" Then
                    designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" : production = "Orion"
                End If
                If Data = "Roller J" Then
                    designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" : production = "JKT"
                End If
                If Data = "Roller S" Then
                    designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" : production = "SP"
                End If

                If Data = "Vertical O" Then
                    designId = "B556E35C-CEAC-40F8-A6CF-156601BD57DA" : production = "Orion"
                End If
                If Data = "Vertical J" Then
                    designId = "B556E35C-CEAC-40F8-A6CF-156601BD57DA" : production = "JKT"
                End If

                If Data = "Panel Glide O" Then
                    designId = "E7959750-5CF8-48E5-9171-CD71B53CDC2F" : production = "Orion"
                End If
                If Data = "Panel Glide J" Then
                    designId = "E7959750-5CF8-48E5-9171-CD71B53CDC2F" : production = "JKT"
                End If

                If Data = "Venetian O" Then
                    designId = "83C7039F-4A2E-4D6A-9389-761664FD9449" : production = "Orion"
                End If
                If Data = "Venetian J" Then
                    designId = "83C7039F-4A2E-4D6A-9389-761664FD9449" : production = "JKT"
                End If

                If Data = "Pelmet O" Then
                    designId = "3734E56C-9FBE-4897-9424-410833B1A1D3" : production = "Orion"
                End If
                If Data = "Pelmet J" Then
                    designId = "3734E56C-9FBE-4897-9424-410833B1A1D3" : production = "JKT"
                End If

                If Data = "Zebra" Then
                    designId = "78400FAF-BA6D-4E8F-923B-68A4CE30C0DD" : production = "Orion"
                End If

                If Data = "Curtain" Then
                    designId = "992EF755-D9A4-4423-8C88-687CD935DF11" : production = "Orion"
                End If

                If Data = "Cellular Shades" Then
                    designId = "0FCA2BF9-2849-4E2A-B04D-DF8519DC536F" : production = "JKT"
                End If

                If Data = "Veri Shades" Then
                    designId = "68359197-6083-4489-863E-EBB1AF056D92" : production = "Orion"
                End If

                Dim thisQuery As String = "SELECT COUNT(OrderDetails.TotalBlinds) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND Products.DesignId = '" + designId + "'"

                If designId = "83C7039F-4A2E-4D6A-9389-761664FD9449" Then
                    thisQuery = "SELECT COUNT(OrderDetails.TotalBlinds) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND (Products.DesignId = '83C7039F-4A2E-4D6A-9389-761664FD9449' OR Products.DesignId = 'B72EEA9A-5FA8-48EC-9307-C66AAAB1AA8F' OR Products.DesignId = 'C105C02C-E587-40FB-8AAD-0F79DD8B63AE')"
                End If

                If designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" Then
                    thisQuery = "SELECT COUNT(OrderDetails.TotalBlinds) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND (Products.DesignId = '50CE8EDF-E106-414C-BDE3-D7AA8F8046D2' OR Products.DesignId = '9BD1C03C-F15F-4323-B7A0-CC988B0E231B')"
                End If

                If designId = "0CB7C37F-D478-49BA-94CB-DCDE83FB84C8" Then
                    thisQuery = "SELECT COUNT(OrderDetails.PanelQty) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND Products.DesignId = '" + designId + "'"
                End If

                result = GetItemData_Integer(thisQuery)
            End If
        Catch ex As Exception
            result = 100000
        End Try
        Return result
    End Function

    Private Function GetProductionOut(Data As String, Start As Date, Last As Date) As Integer
        Dim result As Integer = 0
        Try
            If Not Data = "" Then
                Dim designId As String = String.Empty
                Dim production As String = String.Empty

                If Data = "PVC Shutters Huake" Then
                    designId = "0CB7C37F-D478-49BA-94CB-DCDE83FB84C8" : production = "Orion"
                End If

                If Data = "PVC Shutters Evolve" Then
                    designId = "F70CD0D8-06E5-4C99-B8D8-E9506C1A0F12" : production = "Orion"
                End If

                If Data = "Roller O" Then
                    designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" : production = "Orion"
                End If
                If Data = "Roller J" Then
                    designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" : production = "JKT"
                End If
                If Data = "Roller S" Then
                    designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" : production = "SP"
                End If

                If Data = "Vertical O" Then
                    designId = "B556E35C-CEAC-40F8-A6CF-156601BD57DA" : production = "Orion"
                End If
                If Data = "Vertical J" Then
                    designId = "B556E35C-CEAC-40F8-A6CF-156601BD57DA" : production = "JKT"
                End If

                If Data = "Panel Glide O" Then
                    designId = "E7959750-5CF8-48E5-9171-CD71B53CDC2F" : production = "Orion"
                End If
                If Data = "Panel Glide J" Then
                    designId = "E7959750-5CF8-48E5-9171-CD71B53CDC2F" : production = "JKT"
                End If

                If Data = "Venetian O" Then
                    designId = "83C7039F-4A2E-4D6A-9389-761664FD9449" : production = "Orion"
                End If
                If Data = "Venetian J" Then
                    designId = "83C7039F-4A2E-4D6A-9389-761664FD9449" : production = "JKT"
                End If

                If Data = "Pelmet O" Then
                    designId = "3734E56C-9FBE-4897-9424-410833B1A1D3" : production = "Orion"
                End If
                If Data = "Pelmet J" Then
                    designId = "3734E56C-9FBE-4897-9424-410833B1A1D3" : production = "JKT"
                End If

                If Data = "Zebra" Then
                    designId = "78400FAF-BA6D-4E8F-923B-68A4CE30C0DD" : production = "Orion"
                End If

                If Data = "Curtain" Then
                    designId = "992EF755-D9A4-4423-8C88-687CD935DF11" : production = "Orion"
                End If

                If Data = "Cellular Shades" Then
                    designId = "0FCA2BF9-2849-4E2A-B04D-DF8519DC536F" : production = "JKT"
                End If

                If Data = "Veri Shades" Then
                    designId = "68359197-6083-4489-863E-EBB1AF056D92" : production = "Orion"
                End If

                Dim thisQuery As String = "SELECT COUNT(OrderDetails.TotalBlinds) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND Products.DesignId = '" + designId + "' AND OrderDetails.Paid = 1"

                If designId = "83C7039F-4A2E-4D6A-9389-761664FD9449" Then
                    thisQuery = "SELECT COUNT(OrderDetails.TotalBlinds) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND (Products.DesignId = '83C7039F-4A2E-4D6A-9389-761664FD9449' OR Products.DesignId = 'B72EEA9A-5FA8-48EC-9307-C66AAAB1AA8F' OR Products.DesignId = 'C105C02C-E587-40FB-8AAD-0F79DD8B63AE') AND OrderDetails.Paid = 1"
                End If

                If designId = "50CE8EDF-E106-414C-BDE3-D7AA8F8046D2" Then
                    thisQuery = "SELECT COUNT(OrderDetails.TotalBlinds) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND (Products.DesignId = '50CE8EDF-E106-414C-BDE3-D7AA8F8046D2' OR Products.DesignId = '9BD1C03C-F15F-4323-B7A0-CC988B0E231B') AND OrderDetails.Paid = 1"
                End If

                If designId = "0CB7C37F-D478-49BA-94CB-DCDE83FB84C8" Then
                    thisQuery = "SELECT COUNT(OrderDetails.PanelQty) FROM OrderHeaders INNER JOIN OrderDetails ON OrderHeaders.Id = OrderDetails.HeaderId INNER JOIN Products ON OrderDetails.ProductId = Products.Id WHERE OrderHeaders.Status = 'In Production'  AND CONVERT(DATE, OrderHeaders.SubmittedDate) >= '" + Start + "' AND CONVERT(DATE, OrderHeaders.SubmittedDate) <= '" + Last + "' AND OrderDetails.Active = 1 AND OrderDetails.Production = '" + production + "' AND Products.DesignId = '" + designId + "' AND OrderDetails.Paid = 1"
                End If

                result = GetItemData_Integer(thisQuery)
            End If
        Catch ex As Exception
            result = 100000
        End Try
        Return result
    End Function

    Public Sub UpdateSales(thisString As String)
        Try

        Catch ex As Exception

        End Try
    End Sub
End Class
