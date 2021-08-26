using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public class IncomingTransferReject {
        // Код результата выполнения операции
        [JsonProperty("status")]
        public TransferStatus Status { get; set; }

        // Код ошибки при проведении платежа (пояснение к полю status). Присутствует только при ошибках
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
