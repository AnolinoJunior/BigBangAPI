using Microsoft.AspNetCore.Identity;

namespace BigBangAPI.Localization;

public sealed class DescritorDeErrosIdentityPtBr : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => Erro("Ocorreu um erro inesperado.");
    public override IdentityError ConcurrencyFailure() => Erro("O registro foi alterado por outra operação.");
    public override IdentityError PasswordMismatch() => Erro("Senha inválida.");
    public override IdentityError InvalidToken() => Erro("Token inválido.");
    public override IdentityError LoginAlreadyAssociated() => Erro("Este login já está associado a uma conta.");
    public override IdentityError InvalidUserName(string? userName) => Erro($"O nome de usuário '{userName}' é inválido.");
    public override IdentityError InvalidEmail(string? email) => Erro($"O e-mail '{email}' é inválido.");
    public override IdentityError DuplicateUserName(string userName) => Erro($"O nome de usuário '{userName}' já está em uso.");
    public override IdentityError DuplicateEmail(string email) => Erro($"O e-mail '{email}' já está em uso.");
    public override IdentityError UserAlreadyHasPassword() => Erro("A conta já possui uma senha.");
    public override IdentityError UserLockoutNotEnabled() => Erro("O bloqueio de conta não está habilitado para este usuário.");
    public override IdentityError UserAlreadyInRole(string role) => Erro($"O usuário já pertence à função '{role}'.");
    public override IdentityError UserNotInRole(string role) => Erro($"O usuário não pertence à função '{role}'.");
    public override IdentityError PasswordTooShort(int length) => Erro($"A senha deve ter pelo menos {length} caracteres.");
    public override IdentityError PasswordRequiresNonAlphanumeric() => Erro("A senha deve conter ao menos um caractere que não seja letra ou número.");
    public override IdentityError PasswordRequiresDigit() => Erro("A senha deve conter ao menos um número.");
    public override IdentityError PasswordRequiresLower() => Erro("A senha deve conter ao menos uma letra minúscula.");
    public override IdentityError PasswordRequiresUpper() => Erro("A senha deve conter ao menos uma letra maiúscula.");
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => Erro($"A senha deve conter pelo menos {uniqueChars} caracteres diferentes.");

    private static IdentityError Erro(string descricao) => new() { Description = descricao };
}
