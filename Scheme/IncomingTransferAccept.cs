using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace YooMoney.Scheme {
    public enum TransferStatus {
        // Входящий перевод принят/отвергнут успешно
        [EnumMember(Value = "success")] Success,

        // Отказ в выполнении операции
        [EnumMember(Value = "refused")] Refused,
    }

    public class IncomingTransferAccept {
        // Код результата выполнения операции
        [JsonProperty("status")]
        public TransferStatus Status { get; set; }

        // Код ошибки при проведении платежа (пояснение к полю status). Присутствует только при ошибках
        [JsonProperty("error")]
        public string Error { get; set; }

        // Количество оставшихся попыток принять входящий перевод защищенный кодом протекции. Присутствует только при неверно введенном коде протекции
        [JsonProperty("protection_code_attempts_available")]
        public int AttemptsAvailable { get; set; }

        // Адрес, на который необходимо отправить пользователя для совершения необходимых действий в случае ошибки ext_action_required
        [JsonProperty("ext_action_uri")]
        public string ExtActionUri { get; set; }
    }
}
