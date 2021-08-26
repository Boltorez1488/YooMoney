using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public enum RecipientType {
        // Номер счета получателя в сервисе ЮMoney
        [EnumMember(Value = "account")] Account,
        
        // Номер привязанного мобильного телефона получателя
        [EnumMember(Value = "phone")] Phone,
        
        // Электронная почта получателя перевода
        [EnumMember(Value = "email")] Email,
    }

    public class OperationDetails {
        // Код ошибки, присутствует при ошибке выполнения запроса
        [JsonProperty("error")]
        public string Error { get; set; }

        // Идентификатор операции. Значение параметра соответствует либо значению параметра operation_id ответа метода operation-history либо, в случае если запрашивается история счета плательщика, значению поля payment_id ответа метода process-payment
        [JsonProperty("operation_id")]
        public string OperationId { get; set; }

        // Статус платежа (перевода). Значение параметра соответствует значению поля status ответа метода operation-history
        [JsonProperty("status")]
        public OperationStatus Status { get; set; }

        // Идентификатор шаблона платежа, по которому совершен платеж. Присутствует только для платежей
        [JsonProperty("pattern_id")]
        public string PatternId { get; set; }

        // Направление движения средств
        [JsonProperty("direction")]
        public MoneyDirection Direction { get; set; }

        // Сумма операции (сумма списания со счета)
        [JsonProperty("amount")]
        public double Amount { get; set; }

        // Сумма к получению. Присутствует для исходящих переводов другим пользователям
        [JsonProperty("amount_due")]
        public double AmountDue { get; set; }

        // Сумма комиссии. Присутствует для исходящих переводов другим пользователям
        [JsonProperty("fee")]
        public double Fee { get; set; }

        // Дата и время совершения операции
        [JsonProperty("datetime")]
        public DateTime DateTime { get; set; }

        // Краткое описание операции (название магазина или источник пополнения)
        [JsonProperty("title")]
        public string Title { get; set; }

        // Номер счета отправителя перевода. Присутствует для входящих переводов от других пользователей
        [JsonProperty("sender")]
        public string Sender { get; set; }

        // Идентификатор получателя перевода. Присутствует для исходящих переводов другим пользователям
        [JsonProperty("recipient")]
        public string Recepient { get; set; }

        // Тип идентификатора получателя перевода
        [JsonProperty("recipient_type")]
        public RecipientType RecipientType { get; set; }

        // Сообщение получателю перевода. Присутствует для переводов другим пользователям
        [JsonProperty("message")]
        public string Message { get; set; }

        // Комментарий к переводу или пополнению. Присутствует в истории отправителя перевода или получателя пополнения
        [JsonProperty("comment")]
        public string Comment { get; set; }

        // Перевод защищен кодом протекции. Присутствует для переводов другим пользователям
        [JsonProperty("codepro")]
        public bool IsCodePro { get; set; }

        // Код протекции. Присутствует для исходящих переводов, защищенных кодом протекции
        [JsonProperty("protection_code")]
        public string ProtectionCode { get; set; }

        // Дата и время истечения срока действия кода протекции. Присутствует для входящих и исходящих переводов (от/другим) пользователям, защищенных кодом протекции
        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        // Дата и время приема или отмены перевода, защищенного кодом протекции. Присутствует для входящих и исходящих переводов, защищенных кодом протекции. Если перевод еще не принят или не отвергнут получателем, поле отсутствует
        [JsonProperty("answer_datetime")]
        public DateTime? AnswerDateTime { get; set; }

        // Метка платежа. Присутствует для входящих и исходящих переводов другим пользователям ЮMoney, у которых был указан параметр label вызова request-payment
        [JsonProperty("label")]
        public string Label { get; set; }

        // Детальное описание платежа. Строка произвольного формата, может содержать любые символы и переводы строк. Необязательный параметр
        [JsonProperty("details")]
        public string Details { get; set; }

        // Тип операции
        [JsonProperty("type")]
        public OperationType Type { get; set; }
    }
}
