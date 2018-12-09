using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareOrder.Models.ApiController.Models
{
    [Serializable]
    public class BusinessResult
    {
        private string _orderReferenceNumber;
        private string _errorMessage;        
        public BusinessResult(string orderReferenceNumber, string errorMessage)
        {
            _orderReferenceNumber = orderReferenceNumber;
            _errorMessage = errorMessage;
            IsSuccess = !string.IsNullOrEmpty(orderReferenceNumber) && string.IsNullOrEmpty(errorMessage);
        }
        public bool IsSuccess { get; }
        public string ErrorMessage { get { return _errorMessage; } set { value = _errorMessage; } }
        public string OrderReferenceNumber { get { return _orderReferenceNumber; } set { value = _orderReferenceNumber; } }
    }
}
