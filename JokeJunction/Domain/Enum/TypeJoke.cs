using System.ComponentModel.DataAnnotations;

namespace JokeJunction.Domain.Enum
{
    public enum TypeJoke
    {
        [Display(Name = "Жартівливий")]
        Жартівливий = 0,
        [Display(Name = "Кримінал")]
        Кримінал = 1,
        [Display(Name = "Політика")]
        Політика = 2,
        [Display(Name = "Чорний")]
        Чорний = 3,
        [Display(Name = "IT")]
        IT = 4,

    }
}