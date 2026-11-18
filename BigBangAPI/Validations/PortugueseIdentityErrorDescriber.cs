using Microsoft.AspNetCore.Identity;

namespace BigBangAPI.Validations;

public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() =>
        Describe(base.DefaultError(), "Ocorreu um erro inesperado.");

    public override IdentityError ConcurrencyFailure() =>
        Describe(base.ConcurrencyFailure(), "Os dados foram alterados por outra operação. Tente novamente.");

    public override IdentityError PasswordMismatch() =>
        Describe(base.PasswordMismatch(), "Senha incorreta.");

    public override IdentityError InvalidToken() =>
        Describe(base.InvalidToken(), "Token inválido.");

    public override IdentityError RecoveryCodeRedemptionFailed() =>
        Describe(base.RecoveryCodeRedemptionFailed(), "Não foi possível usar o código de recuperação.");

    public override IdentityError LoginAlreadyAssociated() =>
        Describe(base.LoginAlreadyAssociated(), "Este login já está associado a outro usuário.");

    public override IdentityError InvalidUserName(string? userName) =>
        Describe(base.InvalidUserName(userName), $"O nome de usuário '{userName}' contém caracteres inválidos.");

    public override IdentityError InvalidEmail(string? email) =>
        Describe(base.InvalidEmail(email), $"O e-mail '{email}' é inválido.");

    public override IdentityError DuplicateUserName(string userName) =>
        Describe(base.DuplicateUserName(userName), $"O nome de usuário '{userName}' já está em uso.");

    public override IdentityError DuplicateEmail(string email) =>
        Describe(base.DuplicateEmail(email), $"O e-mail '{email}' já está em uso.");

    public override IdentityError InvalidRoleName(string? role) =>
        Describe(base.InvalidRoleName(role), $"O nome da função '{role}' é inválido.");

    public override IdentityError DuplicateRoleName(string role) =>
        Describe(base.DuplicateRoleName(role), $"A função '{role}' já existe.");

    public override IdentityError UserAlreadyHasPassword() =>
        Describe(base.UserAlreadyHasPassword(), "O usuário já tem uma senha.");

    public override IdentityError UserLockoutNotEnabled() =>
        Describe(base.UserLockoutNotEnabled(), "O bloqueio de conta está desativado para este usuário.");

    public override IdentityError UserAlreadyInRole(string role) =>
        Describe(base.UserAlreadyInRole(role), $"O usuário já pertence à função '{role}'.");

    public override IdentityError UserNotInRole(string role) =>
        Describe(base.UserNotInRole(role), $"O usuário não pertence à função '{role}'.");

    public override IdentityError PasswordTooShort(int length) =>
        Describe(base.PasswordTooShort(length), $"A senha deve ter pelo menos {length} caracteres.");

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) =>
        Describe(base.PasswordRequiresUniqueChars(uniqueChars), $"A senha deve ter pelo menos {uniqueChars} caracteres distintos.");

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        Describe(base.PasswordRequiresNonAlphanumeric(), "A senha deve ter pelo menos um caractere especial.");

    public override IdentityError PasswordRequiresDigit() =>
        Describe(base.PasswordRequiresDigit(), "A senha deve ter pelo menos um algarismo de 0 a 9.");

    public override IdentityError PasswordRequiresLower() =>
        Describe(base.PasswordRequiresLower(), "A senha deve ter pelo menos uma letra minúscula de a a z.");

    public override IdentityError PasswordRequiresUpper() =>
        Describe(base.PasswordRequiresUpper(), "A senha deve ter pelo menos uma letra maiúscula de A a Z.");

    private static IdentityError Describe(IdentityError error, string description)
    {
        error.Description = description;
        return error;
    }
}
