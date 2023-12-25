Imports System.Reflection

Public Class ReflectionUtils

    ''' <summary>
    ''' Gets the value of the given property of the input object.
    ''' </summary>
    ''' <param name="name">The property name to look for.</param>
    ''' <param name="input_object">The object to search on.</param>
    ''' <returns></returns>
    Public Shared Function getPropertyValueByName(name As String, input_object As Object) As String
        For Each property_info As PropertyInfo In input_object.GetType().GetProperties()
            If property_info.CanRead Then
                If property_info.Name.ToLower().Equals(name.ToLower()) Then
                    Return property_info.GetValue(input_object).ToString()
                End If
            End If
        Next

        Return Nothing
    End Function

    Public Shared Function getPropertyValues(input_object As Object) As Dictionary(Of String, String)
        Dim value_list As New Dictionary(Of String, String)

        For Each property_info As PropertyInfo In input_object.GetType().GetProperties()
            If property_info.CanRead Then
                value_list.Add(property_info.Name, property_info.GetValue(input_object).ToString())
            End If
        Next

        Return value_list
    End Function

End Class
