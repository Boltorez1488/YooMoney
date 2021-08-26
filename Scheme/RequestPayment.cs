using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public enum PaymentStatus {
        // Успешное выполнение
        [EnumMember(Value = "success")] Success,

        // Отказ в проведении платежа, объяснение причины отказа содержится в поле error. Это конечное состояние платежа
        [EnumMember(Value = "refused")] Refused,
    }

    public enum CardType {
        [EnumMember(Value = "Visa")] Visa,
        [EnumMember(Value = "MasterCard")] MasterCard,
        [EnumMember(Value = "American Express")] AmericanExpress,
        [EnumMember(Value = "JCB")] Jcb,
    }

    public class CardItem {
        // Идентификатор привязанной к счету банковской карты. Его необходимо указать в методе process-payment для совершения платежа выбранной картой
        [JsonProperty("id")]
        public string Id { get; set; }

        // Фрагмент номера банковской карты. Поле присутствует только для привязанной банковской карты. Может отсутствовать, если неизвестен
        [JsonProperty("pan_fragment")]
        public string PanFragment { get; set; }

        // Тип карты. Может отсутствовать, если неизвестен
        [JsonProperty("type")]
        public CardType Type { get; set; }
    }

    public class MoneySource {
        // Признак того, что данный метод платежа разрешен пользователем
        [JsonProperty("allowed")]
        public bool IsAllowed { get; set; }

        // Признак необходимости требования CVV2/CVC2 кода для авторизации оплаты по банковской карте
        [JsonProperty("csc_required")]
        public bool IsCscRequired { get; set; }

        // Описание банковской карты, привязанной к счету
        [JsonProperty("item")]
        public CardItem Item { get; set; }
    }

    public class RequestPayment {
        // Код результата выполнения операции
        [JsonProperty("status")]
        public PaymentStatus Status { get; set; }

        // Код ошибки при проведении платежа (пояснение к полю status). Присутствует только при ошибках
        [JsonProperty("error")]
        public string Error { get; set; }

        // Доступные для приложения методы проведения платежа. Присутствует только при успешном выполнении метода
        [JsonProperty("money_source")]
        public Dictionary<string, MoneySource> MoneySource { get; set; }

        // Идентификатор запроса платежа. Присутствует только при успешном выполнении метода
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        // Сумма к списанию со счета в валюте счета плательщика (столько заплатит пользователь вместе с комиссией). Присутствует при успешном выполнении метода или ошибке not_enough_funds
        [JsonProperty("contract_amount")]
        public double ContractAmount { get; set; }

        // Текущий баланс счета пользователя. Присутствует при выполнении следующих условий: метод выполнен успешно; токен авторизации обладает правом account-info
        [JsonProperty("balance")]
        public double Balance { get; set; }

        // Статус пользователя
        [JsonProperty("recipient_account_status")]
        public AccountStatus RecipientAccountStatus { get; set; }

        // Тип счета получателя. Параметр присутствует при успешном выполнении метода в случае перевода средств на счет в ЮMoney другого пользователя
        [JsonProperty("recipient_account_type")]
        public AccountType RecipientAccountType { get; set; }

        // Код протекции для данного перевода. Параметр присутствует, если был указан входной параметр codepro=true. Строка из 4-х десятичных цифр, может включать в себя ведущие нули. Параметр должен обрабатываться как строка
        [JsonProperty("protection_code")]
        public string ProtectionCode { get; set; }

        // Адрес, на который необходимо отправить пользователя для разблокировки счета. Поле присутствует в случае ошибки account_blocked
        [JsonProperty("account_unblock_uri")]
        public string AccountUnblockUri { get; set; }

        // Адрес, на который необходимо отправить пользователя для совершения необходимых действий в случае ошибки ext_action_required
        [JsonProperty("ext_action_uri")]
        public string ExtActionUri { get; set; }
    }
}
