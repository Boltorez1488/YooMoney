using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public enum OperationStatus {
        // Платеж завершен успешно
        [EnumMember(Value = "success")] Success,

        // Платеж отвергнут получателем или отменен отправителем
        [EnumMember(Value = "refused")] Refused,

        // Платеж не завершен, перевод не принят получателем или ожидает ввода кода протекции
        [EnumMember(Value = "in_progress")] InProgress,
    }

    public enum MoneyDirection {
        // Приход
        [EnumMember(Value = "in")] In,

        // Расход
        [EnumMember(Value = "out")] Out,
    }

    public enum OperationType {
        // Исходящий платеж в магазин
        [EnumMember(Value = "payment-shop")] PaymentShop,

        // Исходящий P2P-перевод любого типа
        [EnumMember(Value = "outgoing-transfer")] OutgoingTransfer,

        // Зачисление
        [EnumMember(Value = "deposition")] Deposition,

        // Входящий перевод или перевод до востребования
        [EnumMember(Value = "incoming-transfer")] IncomingTransfer,

        // Входящий перевод с кодом протекции
        [EnumMember(Value = "incoming-transfer-protected")] IncomingTransferProtected,
    }

    public enum OperationReqType {
        // Пополнение счета (приход)
        [EnumMember(Value = "deposition")] Deposition,

        // Платежи со счета (расход)
        [EnumMember(Value = "payment")] IncomingTransfer,

        // Не принятые входящие P2P-переводы любого типа
        [EnumMember(Value = "incoming-transfers-unaccepted")] IncomingTransfersUnaccepted,
    }

    public class OperationHistory {
        // Код ошибки. Присутствует при ошибке выполнения запроса
        [JsonProperty("error")]
        public string Error { get; set; }

        // Порядковый номер первой записи на следующей странице истории операций. Присутствует в случае наличия следующей страницы истории
        [JsonProperty("next_record")]
        public string NextRecord { get; set; }

        // Список операций
        [JsonProperty("operations")]
        public List<Operation> Operations { get; set; } = new();
    }

    public class Operation {
        // Идентификатор операции
        [JsonProperty("operation_id")]
        public string OperationId { get; set; }

        // Статус платежа (перевода)
        [JsonProperty("status")]
        public OperationStatus Status { get; set; }

        // Дата и время совершения операции
        [JsonProperty("datetime")]
        public DateTime DateTime { get; set; }

        // Краткое описание операции (название магазина или источник пополнения)
        [JsonProperty("title")]
        public string Title { get; set; }

        // Идентификатор шаблона, по которому совершен платеж. Присутствует только для платежей
        [JsonProperty("pattern_id")]
        public string PatternId { get; set; }

        // Направление движения средств
        [JsonProperty("direction")]
        public MoneyDirection Direction { get; set; }

        // Сумма операции
        [JsonProperty("amount")]
        public double Amount { get; set; }

        // Метка платежа. Присутствует для входящих и исходящих переводов другим пользователям ЮMoney, у которых был указан параметр label вызова request-payment
        [JsonProperty("label")]
        public string Label { get; set; }

        // Тип операции
        [JsonProperty("type")]
        public OperationType Type { get; set; }
    }
}
