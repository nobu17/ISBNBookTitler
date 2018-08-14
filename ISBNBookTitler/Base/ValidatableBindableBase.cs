using LogService;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ISBNBookTitler
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly ILogWrite _logger;

        private readonly ErrorsContainer<string> Errors;

        public ValidatableBindableBase()
        {
            this.Errors = new ErrorsContainer<string>(this.OnErrorsChanged);

            _logger = new NLogService();
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return Errors.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get { return Errors.HasErrors; }
        }


        private void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        protected void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };
            var validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateProperty(value, context, validationErrors))
            {
                this.Errors.SetErrors(propertyName, validationErrors.Select(error => error.ErrorMessage));
            }
            else
            {
                this.Errors.ClearErrors(propertyName);
            }
        }

        public bool ValidateAllObjects()
        {
            if (!this.HasErrors)
            {
                var context = new ValidationContext(this);
                var validationErrors = new List<ValidationResult>();
                if (Validator.TryValidateObject(this, context, validationErrors))
                {
                    return true;
                }

                var errors = validationErrors.Where(_ => _.MemberNames.Any()).GroupBy(_ => _.MemberNames.First());
                foreach (var error in errors)
                {
                    this.Errors.SetErrors(error.Key, error.Select(_ => _.ErrorMessage));
                }
            }
            return false;
        }

        #region logging


        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception exception = null)
        {
            _logger.Error(message, exception);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        #endregion
    }
}
