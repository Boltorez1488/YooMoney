using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YooMoney.Core;
using YooMoney.Scheme;

namespace YooMoney {
    public class YooApi {
        public string Token { get; }
        public string ApiUri => "https://yoomoney.ru/api";

        public YooApi(string token) {
            Token = token;
        }

        /// <summary>
        /// Получение информации о состоянии счета пользователя.
        /// </summary>
        /// <returns></returns>
        public async Task<AccountInfo> AccountInfo() {
            using var client = new ApiClient(ApiUri, Token);
            var r = await client.PostAsync("account-info");
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<AccountInfo>(await r.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Метод позволяет просматривать историю операций (полностью или частично) в постраничном режиме. Записи истории выдаются в обратном хронологическом порядке: от последних к более ранним.
        /// </summary>
        /// <param name="records">Количество запрашиваемых записей истории операций. Допустимые значения: от 1 до 100</param>
        /// <param name="label">Отбор платежей по значению метки. Выбираются платежи, у которых указано заданное значение параметра label вызова request-payment.</param>
        /// <param name="startRecord">Если параметр присутствует, то будут отображены операции, начиная с номера start_record. Операции нумеруются с 0.</param>
        /// <param name="from">Вывести операции от момента времени (операции, равные from, или более поздние). Если параметр отсутствует, выводятся все операции.</param>
        /// <param name="to">Вывести операции до момента времени (операции более ранние, чем to). Если параметр отсутствует, выводятся все операции.</param>
        /// <param name="details">Показывать подробные детали операции. По умолчанию false. Для отображения деталей операции требуется наличие права operation-details.</param>
        /// <param name="type">Перечень типов операций, которые требуется отобразить.</param>
        /// <returns></returns>
        public async Task<OperationHistory> OperationHistory(int records = 1, string label = null, int startRecord = 0, DateTime? from = null, DateTime? to = null, bool details = false, OperationReqType? type = null) {
            using var client = new ApiClient(ApiUri, Token);
            var p = new Dictionary<string, string> {
                ["records"] = records.ToString()
            };
            if (details) {
                p["details"] = "true";
            }
            if (type.HasValue) {
                p["type"] = Utils.ToEnumString(type.Value);
            }
            if (startRecord > 0) {
                p["start_record"] = startRecord.ToString();
            }
            if (label != null) {
                p["label"] = label;
            }
            if (from.HasValue) {
                p["from"] = from.Value.ToString("O");
            }
            if (to.HasValue) {
                p["till"] = to.Value.ToString("O");
            }
            var r = await client.PostAsync("operation-history", p);
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<OperationHistory>(await r.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Позволяет получить детальную информацию об операции из истории.
        /// </summary>
        /// <param name="operationId">Идентификатор операции. Значение параметра следует указывать как значение параметра operation_id ответа метода operation-history или значение поля payment_id ответа метода process-payment, если запрашивается история счета плательщика.</param>
        /// <returns></returns>
        public async Task<OperationDetails> OperationDetails(string operationId) {
            using var client = new ApiClient(ApiUri, Token);
            var r = await client.PostAsync("operation-details", new Dictionary<string, string> {
                ["operation_id"] = operationId
            });
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<OperationDetails>(await r.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Прием входящих переводов, защищенных кодом протекции, и переводов до востребования. Количество попыток приема входящего перевода с кодом протекции ограничено.При исчерпании количества попыток, перевод автоматически отвергается(перевод возвращается отправителю).
        /// </summary>
        /// <param name="operationId">Идентификатор операции, значение параметра operation_id ответа метода operation-history.</param>
        /// <param name="protectionCode">Код протекции. Строка из 4-х десятичных цифр. Указывается для входящего перевода, защищенного кодом протекции. Для переводов до востребования отсутствует.</param>
        /// <returns></returns>
        public async Task<IncomingTransferAccept> IncomingTransferAccept(string operationId, string protectionCode) {
            using var client = new ApiClient(ApiUri, Token);
            var r = await client.PostAsync("incoming-transfer-accept", new Dictionary<string, string> {
                ["operation_id"] = operationId,
                ["protection_code"] = protectionCode,
            });
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<IncomingTransferAccept>(await r.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Отмена входящих переводов, защищенных кодом протекции, и переводов до востребования. При отмене перевода он возвращается отправителю.
        /// </summary>
        /// <param name="operationId">Идентификатор операции, значение параметра operation_id ответа метода operation-history.</param>
        /// <returns></returns>
        public async Task<IncomingTransferAccept> IncomingTransferReject(string operationId) {
            using var client = new ApiClient(ApiUri, Token);
            var r = await client.PostAsync("incoming-transfer-reject", new Dictionary<string, string> {
                ["operation_id"] = operationId
            });
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<IncomingTransferAccept>(await r.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Создание платежа, проверка параметров и возможности приема платежа магазином или перевода средств на счет пользователя ЮMoney.
        /// </summary>
        /// <param name="toEmailPhoneWallet">Идентификатор получателя перевода (номер счета, номер телефона или email).</param>
        /// <param name="amount">Сумма к оплате (столько заплатит отправитель).</param>
        /// <param name="amountDue">Сумма к получению (придет на счет получателя счет после оплаты).</param>
        /// <param name="label">Метка платежа. Необязательный параметр.</param>
        /// <param name="comment">Комментарий к переводу, отображается в истории отправителя.</param>
        /// <param name="message">Комментарий к переводу, отображается получателю.</param>
        /// <param name="codepro">Значение параметра true — признак того, что перевод защищен кодом протекции. По умолчанию параметр отсутствует (обычный перевод).</param>
        /// <param name="expirePeriod">Число дней, в течении которых: получатель перевода может ввести код протекции и получить перевод на свой счет, получатель перевода до востребования может получить перевод. Значение параметра должно находиться в интервале от 1 до 365. Необязательный параметр. По умолчанию 1.</param>
        /// <returns></returns>
        public async Task<RequestPayment> RequestPayment(string toEmailPhoneWallet, double amount, double amountDue = 0, string label = null, string comment = "", string message = "", bool codepro = false, int? expirePeriod = null) {
            using var client = new ApiClient(ApiUri, Token);
            var p = new Dictionary<string, string> {
                ["pattern_id"] = "p2p",
                ["to"] = toEmailPhoneWallet,
                ["amount"] = amount.ToString(CultureInfo.CurrentCulture),
                ["amount_due"] = amountDue.ToString(CultureInfo.CurrentCulture),
                ["comment"] = comment,
                ["message"] = message
            };
            if (label != null) {
                p["label"] = label;
            }
            if (codepro) {
                p["codepro"] = "true";
            }
            if (expirePeriod.HasValue) {
                p["expire_period"] = expirePeriod.Value.ToString();
            }
            var r = await client.PostAsync("request-payment", p);
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<RequestPayment>(await r.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Подтверждение платежа, ранее созданного методом request-payment. Указание метода проведения платежа.
        /// </summary>
        /// <param name="requestId">Идентификатор запроса, полученный из ответа метода request-payment.</param>
        /// <param name="moneySource">Запрашиваемый метод проведения платежа: wallet — со счета пользователя, идентификатор привязанной к счету карты(значение поля id описания банковской карты). По умолчанию: wallet</param>
        /// <param name="csc">Card Security Code, CVV2/CVC2-код привязанной банковской карты пользователя. Параметр следует указывать только при платеже с привязанной банковской карты.</param>
        /// <param name="extAuthSuccessUri">Адрес страницы возврата при успехе аутентификации платежа банковской картой по 3‑D Secure. Указывается, если приложение поддерживает аутентификацию по 3‑D Secure. Обязательный параметр для такой аутентификации.</param>
        /// <param name="extAuthFailUri">Адрес страницы возврата при отказе в аутентификации платежа банковской картой по 3‑D Secure. Указывается, если приложение поддерживает аутентификацию по 3‑D Secure. Обязательный параметр для такой аутентификации.</param>
        /// <returns></returns>
        public async Task<ProcessPayment> ProcessPayment(string requestId, string moneySource = "wallet", string csc = null, string extAuthSuccessUri = null, string extAuthFailUri = null) {
            using var client = new ApiClient(ApiUri, Token);
            var p = new Dictionary<string, string> {
                ["request_id"] = requestId,
                ["money_source"] = moneySource
            };
            if (csc != null) {
                p["csc"] = csc;
            }
            if (extAuthSuccessUri != null) {
                p["ext_auth_success_uri"] = extAuthSuccessUri;
            }
            if (extAuthFailUri != null) {
                p["ext_auth_fail_uri"] = extAuthFailUri;
            }
            var r = await client.PostAsync("process-payment", p);
            return !r.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<ProcessPayment>(await r.Content.ReadAsStringAsync());
        }
    }
}
