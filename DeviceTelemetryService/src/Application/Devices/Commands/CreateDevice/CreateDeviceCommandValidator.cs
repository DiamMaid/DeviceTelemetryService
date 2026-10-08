namespace Application.Devices.Commands.CreateDevice;

using FluentValidation;

/// <summary>
/// Валидатор команды создания устройства.
/// </summary>
public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator()
    {
        RuleFor(x => x.DeviceDto.Name)
            .NotEmpty().WithMessage("Имя устройства обязательно")
            .MaximumLength(100).WithMessage("Имя устройства не должно превышать 100 символов");

        RuleFor(x => x.DeviceDto.Type)
            .IsInEnum().WithMessage("Недопустимый тип датчика");

        RuleFor(x => x.DeviceDto.Location)
            .MaximumLength(200).WithMessage("Местоположение не должно превышать 200 символов");
    }
}
