using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.EF.Entities;

/// <summary>
/// Represents a currency code stored in the database.
/// </summary>
[Table("currency_code")]
public class CurrencyCode
{
    [Key]
    [Column("currency_code_id")]
    public int Id { get; set; }

    [Column("currency_code")]
    [StringLength(5)]
    public string CurrenciesCode { get; set; }

    public IList<BankAccount> BankAccounts { get; set; }
}
