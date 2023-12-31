﻿namespace WebApi.Dtos;

public class AddPeriodResponse
{
    public Guid Id { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public string Name { get; set; }
}
