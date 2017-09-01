using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Domain.Messages;
using QZCHY.Services.Events;
using System;
using System.Collections.Generic;

namespace QZCHY.Services.Messages
{
    public class MessageTokenProvider:IMessageTokenProvider
    {
        #region Fields

        //private readonly IDateTimeHelper _dateTimeHelper;
        //private readonly IPriceFormatter _priceFormatter;
        private readonly IWorkContext _workContext;
        //private readonly IDownloadService _downloadService;
        //private readonly IOrderService _orderService;
        //private readonly IPaymentService _paymentService;
        //private readonly IProductAttributeParser _productAttributeParser;
        //private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        //private readonly IStoreService _storeService;
        //private readonly IStoreContext _storeContext;

        private readonly MessageTemplatesSettings _templatesSettings;

        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public MessageTokenProvider(
            //IDateTimeHelper dateTimeHelper,
            //IPriceFormatter priceFormatter,
            IWorkContext workContext,
            //IDownloadService downloadService,
            //IOrderService orderService,
            //IPaymentService paymentService,
            //IStoreService storeService,
            //IStoreContext storeContext,
            //IProductAttributeParser productAttributeParser,
            //IAddressAttributeFormatter addressAttributeFormatter,
            MessageTemplatesSettings templatesSettings,
            IEventPublisher eventPublisher)
        {         
            //this._dateTimeHelper = dateTimeHelper;
            //this._priceFormatter = priceFormatter; 
            this._workContext = workContext;
            //this._downloadService = downloadService;
            //this._orderService = orderService;
            //this._paymentService = paymentService;
            //this._productAttributeParser = productAttributeParser;
            //this._addressAttributeFormatter = addressAttributeFormatter;
            //this._storeService = storeService;
            //this._storeContext = storeContext;

            this._templatesSettings = templatesSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        public virtual void AddStoreTokens(IList<Token> tokens,  EmailAccount emailAccount)
        {
            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");

            //tokens.Add(new Token("Store.Name", store.Name));
            //tokens.Add(new Token("Store.URL", store.Url, true));
            //tokens.Add(new Token("Store.Email", emailAccount.Email));
            //tokens.Add(new Token("Store.CompanyName", store.CompanyName));
            //tokens.Add(new Token("Store.CompanyAddress", store.CompanyAddress));
            //tokens.Add(new Token("Store.CompanyPhoneNumber", store.CompanyPhoneNumber));
            //tokens.Add(new Token("Store.CompanyVat", store.CompanyVat));

            //event notification
            //_eventPublisher.EntityTokensAdded(store, tokens);
        }

        public virtual void AddAccountUserTokens(IList<Token> tokens, AccountUser property)
        { 
            tokens.Add(new Token("AccountUser.Username", property.UserName));
            //tokens.Add(new Token("AccountUser.FullName", property.GetFullName()));
            //tokens.Add(new Token("AccountUser.FirstName", property.GetAttribute<string>(SystemAccountUserAttributeNames.FirstName)));
            //tokens.Add(new Token("AccountUser.LastName", property.GetAttribute<string>(SystemAccountUserAttributeNames.LastName)));
            //tokens.Add(new Token("AccountUser.VatNumber", property.GetAttribute<string>(SystemAccountUserAttributeNames.VatNumber)));
            //tokens.Add(new Token("AccountUser.VatNumberStatus", ((VatNumberStatus)property.GetAttribute<int>(SystemAccountUserAttributeNames.VatNumberStatusId)).ToString()));



            //note: we do not use SEO friendly URLS because we can get errors caused by having .(dot) in the URL (from the email address)
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            //string passwordRecoveryUrl = string.Format("{0}passwordrecovery/confirm?token={1}&email={2}", GetStoreUrl(), property.GetAttribute<string>(SystemAccountUserAttributeNames.PasswordRecoveryToken), HttpUtility.UrlEncode(property.Email));
            //string accountActivationUrl = string.Format("{0}property/activation?token={1}&email={2}", GetStoreUrl(), property.GetAttribute<string>(SystemAccountUserAttributeNames.AccountActivationToken), HttpUtility.UrlEncode(property.Email));
            //var wishlistUrl = string.Format("{0}wishlist/{1}", GetStoreUrl(), property.AccountUserGuid);
            //tokens.Add(new Token("AccountUser.PasswordRecoveryURL", passwordRecoveryUrl, true));
            //tokens.Add(new Token("AccountUser.AccountActivationURL", accountActivationUrl, true));
            //tokens.Add(new Token("Wishlist.URLForAccountUser", wishlistUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(property, tokens);
        }


    }
}
