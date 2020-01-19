
using FluentValidation;
using FluentValidation.Results;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ReactiveUI.FluentValidation
{
    public class ReactiveValidationObject : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Events
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Fields
        private readonly IValidator validator;
        private IEnumerable<ValidationFailure> Errors;
        private bool _hasErrors;
        private bool _isValid;

        //protected readonly IObservable<bool> isValid;
        #endregion

        #region Properties
        public bool HasErrors { get => _hasErrors; private set => this.RaiseIfPropertyChanged(ref _hasErrors, value); }


        public bool IsValid { get => _isValid; private set => this.RaiseIfPropertyChanged(ref _isValid, value); }
        #endregion

        #region Constructors
        public ReactiveValidationObject(IValidator _validator)
        {
            validator = _validator ?? throw new ArgumentNullException(nameof(_validator));
            //isValid = this.WhenAnyValue(x => x.IsValid)
            //    .Skip(1)
            //    .Select(_ => !HasErrors)
            //    .StartWith(false);
        }
        #endregion

        #region Public Methods
        public IEnumerable GetErrors(string propertyName) => Errors.Where(x => x.PropertyName == propertyName);

        protected bool RaiseValidation(params string[] propertyName)
        {
            var ret = validator.Validate(this);
            Errors = ret.Errors;
            HasErrors = ret.Errors.Count > 0;
            IsValid = ret.IsValid;
            foreach (var item in propertyName)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(item));
            }
            return ret.IsValid; ;
        }
        #endregion

        protected virtual void RaiseAndValidateAndSetIfChanged<T>(ref T val, T value, ref T oldValue, [CallerMemberName] string name = null)
        {
            if (val.Equals(value) == false)
            {
                oldValue = val;
                val = value;
                if (RaiseValidation(name))
                {


                }
                RaisePropertyChanged(name);
            }
        }

        protected virtual void RaiseAndValidateAndSetIfChanged<T>(ref T val, T value, [CallerMemberName] string name = null)
        {
            if (val.Equals(value) == false)
            {
     
                val = value;
                if (RaiseValidation(name))
                {


                }
                RaisePropertyChanged(name);
            }
        }

        public bool SuppressChange { get; set; }


        protected void RaisePropertyChanged<T>(ref T val, T value, [CallerMemberName] string propertyName = "")
        {
            val = value;

            RaisePropertyChanged(propertyName);

        }

        protected void RaiseIfPropertyChanged<T>(ref T val, T value, [CallerMemberName] string propertyName = "")
        {
            if (val.Equals(value) == false)
            {
                RaisePropertyChanged(ref val, value, propertyName);
            }
        }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (SuppressChange != true)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

