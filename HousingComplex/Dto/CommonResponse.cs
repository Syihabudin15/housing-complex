﻿namespace HousingComplex.Dto;

public class CommonResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}