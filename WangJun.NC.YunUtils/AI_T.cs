using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Nlp.V20190408;
using TencentCloud.Nlp.V20190408.Models;
using System.Linq;

namespace WangJun.NC.YunUtils
{
    public class AI_T
    {
        private string secretId = "1";
        private string secretKey = "1";

        public static AI_T GetInst(string secretId = "1", string secretKey = "1")
        {
            var inst = new AI_T();
            inst.secretId = secretId;
            inst.secretKey = secretKey;
            return inst;
        }

        /// <summary>
        /// 关键词提取
        /// </summary>
        /// <returns></returns>
        public RES KeywordsExtraction(string input)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("nlp.ap-shanghai.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                NlpClient client = new NlpClient(cred, "ap-guangzhou", clientProfile);
                KeywordsExtractionRequest req = new KeywordsExtractionRequest();
                string strParams = JSON.ToJson(new { Text = input });
                req = KeywordsExtractionRequest.FromJsonString<KeywordsExtractionRequest>(strParams);
                KeywordsExtractionResponse resp = client.KeywordsExtraction(req).
                    ConfigureAwait(false).GetAwaiter().GetResult();
                return RES.OK(resp.Keywords.ToList());
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex.Message);
            }
        }

        /// <summary>
        /// 自动摘要
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RES AutoSummarization(string input)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("nlp.ap-shanghai.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                NlpClient client = new NlpClient(cred, "ap-guangzhou", clientProfile);
                AutoSummarizationRequest req = new AutoSummarizationRequest();
                string strParams = JSON.ToJson(new { Text = input });
                req = AutoSummarizationRequest.FromJsonString<AutoSummarizationRequest>(strParams);
                AutoSummarizationResponse resp = client.AutoSummarization(req).
                    ConfigureAwait(false).GetAwaiter().GetResult();
                return RES.OK(resp);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex.Message);
            }
        }

        /// <summary>
        /// 情感分析
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RES SentimentAnalysis(string input)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("nlp.ap-shanghai.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                NlpClient client = new NlpClient(cred, "ap-guangzhou", clientProfile);
                SentimentAnalysisRequest req = new SentimentAnalysisRequest();
                string strParams = JSON.ToJson(new { Text = input });
                req = SentimentAnalysisRequest.FromJsonString<SentimentAnalysisRequest>(strParams);
                SentimentAnalysisResponse resp = client.SentimentAnalysis(req).
                    ConfigureAwait(false).GetAwaiter().GetResult();
                return RES.OK(resp);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex.Message);

            }
        }

        /// <summary>
        /// 文本分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RES TextClassification(string input)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("nlp.ap-shanghai.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                NlpClient client = new NlpClient(cred, "ap-guangzhou", clientProfile);
                TextClassificationRequest req = new TextClassificationRequest();
                string strParams = JSON.ToJson(new { Text = input });
                req = TextClassificationRequest.FromJsonString<TextClassificationRequest>(strParams);
                TextClassificationResponse resp = client.TextClassification(req).
                    ConfigureAwait(false).GetAwaiter().GetResult();
                return RES.OK(resp);
            }
            catch (Exception ex)
            {
                return RES.FAIL(ex.Message);

            }
        }
    }
}
