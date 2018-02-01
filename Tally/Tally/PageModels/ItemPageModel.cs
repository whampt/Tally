using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FreshMvvm;
using Tally;
using Tally.Models;
using System.Windows.Input;

namespace Tally.PageModels
{
    class ItemPageModel : FreshBasePageModel
    {
        private Repository _repository = FreshIOC.Container.Resolve<Repository>();

        private Item _item;

        public string ItemName
        {
            get { return _item.Name; }
            set { _item.Name = value; RaisePropertyChanged(); }
        }

        public string ItemCost
        {
            get { return _item.Cost; }
            set { _item.Cost = value; RaisePropertyChanged(); }
        }
        public override void Init(object initData)
        {
            _item = initData as Item;
            if (_item == null) _item = new Item();
            base.Init(initData);
            RaisePropertyChanged(nameof(ItemName));
            RaisePropertyChanged(nameof(ItemCost));
        }

        /// <summary>
        /// Command associated with the save action.
        /// Persists the Item to the database if the Item is valid.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () => {
                    if (_item != null && _item.Name != null && _item.IsValid())
                    {
                        await _repository.CreateItem(_item);
                        await CoreMethods.PopPageModel(_item);
                    }
                });
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Command(async () => {
                    if ( _item.IsValid())
                    {
                        await _repository.DeleteItemAsync(_item);
                        await CoreMethods.PopPageModel(_item);
                    }
                });
            }

        }

    }
}
