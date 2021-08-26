using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public enum ProcessPaymentStatus {
        Undefined,

        // Успешное выполнение (платеж проведен). Это конечное состояние платежа
        [EnumMember(Value = "success")] Success,

        // Отказ в проведении платежа. Причина отказа возвращается в поле error. Это конечное состояние платежа
        [EnumMember(Value = "refused")] Refused,

        // Авторизация платежа не завершена. Приложению следует повторить запрос с теми же параметрами спустя некоторое время
        [EnumMember(Value = "in_progress")] InProgress,

        // Для завершения авторизации платежа с использованием банковской карты требуется аутентификация по технологии 3‑D Secure
        [EnumMember(Value = "ext_auth_required")] ExtAuthRequired,
    }

    public class ProcessPayment {
        // Код результата выполнения операции
        [JsonProperty("status")]
        public ProcessPaymentStatus Status { get; set; }

        // Код ошибки при проведении платежа (пояснение к полю status). Присутствует только при ошибках
        [JsonProperty("error")]
        public string Error { get; set; }

        // Идентификатор проведенного платежа. Присутствует только при успешном выполнении метода. Этот параметр соответствует параметру operation_id в методах operation-history и operation-details истории плательщика
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }

        // Баланс счета пользователя после проведения платежа. Присутствует только при выполнении следующих условий: метод выполнен успешно; токен авторизации обладает правом account-info
        [JsonProperty("balance")]
        public double Balance { get; set; }

        // Номер транзакции магазина в ЮMoney. Присутствует при успешном выполнении платежа в магазин
        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }

        // Номер счета плательщика. Присутствует при успешном переводе средств на счет другого пользователя ЮMoney
        [JsonProperty("payer")]
        public string Payer { get; set; }

        // Номер счета получателя. Присутствует при успешном переводе средств на счет другого пользователя ЮMoney
        [JsonProperty("payee")]
        public string Payee { get; set; }

        // Сумма, поступившая на счет получателя. Присутствует при успешном переводе средств на счет другого пользователя ЮMoney
        [JsonProperty("credit_amount")]
        public double CreditAmount { get; set; }

        // Адрес, на который необходимо отправить пользователя для разблокировки счета. Поле присутствует в случае ошибки account_blocked
        [JsonProperty("account_unblock_uri")]
        public string AccountUnblockUri { get; set; }

        // Адрес страницы аутентификации банковской карты по 3‑D Secure на стороне банка-эмитента. Поле присутствует, если для завершения транзакции с использованием банковской карты требуется аутентификация по 3‑D Secure
        [JsonProperty("acs_uri")]
        public string AcsUri { get; set; }

        // Параметры аутентификации карты по 3‑D Secure в формате коллекции «имя-значение». Поле присутствует, если для завершения транзакции с использованием банковской карты требуется аутентификация по 3‑D Secure
        [JsonProperty("acs_params")]
        public Dictionary<string, string> AcsParams { get; set; }

        // Рекомендуемое время, спустя которое следует повторить запрос, в миллисекундах. Поле присутствует при status=in_progress
        [JsonProperty("next_retry")]
        public long NextRetry { get; set; }
    }
}
