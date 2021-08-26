using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public enum AccountStatus {
        // Анонимный счет
        [EnumMember(Value = "anonymous")] Anonymous,

        // Именной счет
        [EnumMember(Value = "named")] Named,

        // Идентифицированный счет
        [EnumMember(Value = "identified")] Identified 
    }

    public enum AccountType {
        // Счет пользователя в ЮMoney
        [EnumMember(Value = "personal")] Personal,

        // Профессиональный счет в ЮMoney
        [EnumMember(Value = "professional")] Professional, 
    }

    public class AccountInfo {
        // Номер счёта
        [JsonProperty("account")]
        public string Account { get; set; }

        // Баланс
        [JsonProperty("balance")]
        public double Balance { get; set; }

        // Валюта
        [JsonProperty("currency")]
        public string Currency { get; set; }

        // Статус пользователя
        [JsonProperty("account_status")]
        public AccountStatus AccountStatus { get; set; }

        // Тип счёта
        [JsonProperty("account_type")]
        public AccountType AccountType { get; set; }
    }
}
