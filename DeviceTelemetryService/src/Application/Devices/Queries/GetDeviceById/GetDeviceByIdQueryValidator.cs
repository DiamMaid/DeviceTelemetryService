namespace Application.Devices.Queries.GetDeviceById;

using FluentValidation;

/// <summary>
/// Валидатор запроса получения устройства по ID.
/// </summary>
public class GetDeviceByIdQueryValidator : AbstractValidator<GetDeviceByIdQuery>
{
    public GetDeviceByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID устройства обязателен")
            .NotEqual(Guid.Empty).WithMessage("ID устройства не может быть пустым");
    }
}