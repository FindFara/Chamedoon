using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Account.Register.ViewModel;

public class RegisterUser_VM : IMapFrom<User>
{

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
    //[Remote("IsEmailInUse", "Account", HttpMethod = "POST")]
    public string Email { get; set; }

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    //[Display(Name = "تکرار کلمه عبور")]
    //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //[MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    //[Compare("Password", ErrorMessage = "کلمه های عبور مغایرت دارند")]
    //[DataType(DataType.Password)]
    //public string RePassword { get; set; }
}




