using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using Tally.Models;
using Xamarin.Forms;
using System.Linq;

namespace Tally.PageModels
{
    class ItemListPageModel : FreshBasePageModel
    {
        private Repository _repository = FreshIOC.Container.Resolve<Repository>();
        private Item _selectedItem = null;
        public string ItemSubTotal { get; private set; }
        public string ItemTotal { get; private set; }
        /// <summary>
        /// Collection used for binding to the Page's contact list view.
        /// </summary>
        public ObservableCollection<Item> Items { get; private set; }

        /// <summary>
        /// Used to bind with the list view's SelectedItem property.
        /// Calls the EditContactCommand to start the editing.
        /// </summary>
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (value != null) EditContactCommand.Execute(value);
            }
        }

        public ItemListPageModel()
        {
            Items = new ObservableCollection<Item>();
            
        }
        /// <summary>
        /// Called whenever the page is navigated to.
        /// Here we are ignoring the init data and just loading the contacts.
        /// </summary>
        public override void Init(object initData)
        {
            LoadItems();
            if (Items.Count() < 1)
            {
                CreateSampleData();
            }
        }

        /// <summary>
        /// Called whenever the page is navigated to, but from a pop action.
        /// Here we are just updating the contact list with most recent data.
        /// </summary>
        /// <param name="returnedData"></param>
        public override void ReverseInit(object returnedData)
        {
            LoadItems();
            base.ReverseInit(returnedData);
        }

        /// <summary>
        /// Command associated with the add contact action.
        /// Navigates to the ContactPageModel with no Init object.
        /// </summary>
        public ICommand AddItemCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<ItemPageModel>();
                    RaisePropertyChanged(nameof(ItemTotal));
                    RaisePropertyChanged(nameof(ItemSubTotal));
                });
            }
        }

        /// <summary>
        /// Command associated with the edit contact action.
        /// Navigates to the ContactPageModel with the selected contact as the Init object.
        /// </summary>
        public ICommand EditContactCommand
        {
            get
            {
                return new Command(async (contact) =>
                {
                    await CoreMethods.PushPageModel<ItemPageModel>(contact);
                    RaisePropertyChanged(nameof(ItemTotal));
                    RaisePropertyChanged(nameof(ItemSubTotal));
                });
            }
        }

        /// <summary>
        /// Repopulate the collection with updated contacts data.
        /// Note: For simplicity, we wait for the async db call to complete,
        /// recommend making better use of the async potential.
        /// </summary>
        private void LoadItems()
        {
            Items.Clear();
            Task<List<Item>> getItemsTask = _repository.GetAllItems();
            getItemsTask.Wait();
            foreach (var item in getItemsTask.Result)
            {
                Items.Add(item);
            }
            ItemSubTotal = SubTotal();
            ItemTotal = Total(ItemSubTotal);
            RaisePropertyChanged(nameof(ItemTotal));
            RaisePropertyChanged(nameof(ItemSubTotal));
        }

        /// <summary>
        /// Uses the SQLite Async capability to insert sample data on multiple threads.
        /// </summary>
        private void CreateSampleData()
        {
            var contact1 = new Item
            {
                Name = "Thing One",
                Cost = "0"
            };



            var task1 = _repository.CreateItem(contact1);


            // Don't proceed until all the async inserts are complete.
            var allTasks = Task.WhenAll(task1);
            allTasks.Wait();

            LoadItems();
        }
        public ICommand DeleteAllCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (await CoreMethods.DisplayAlert("Delete List", "Are you Sure?", "Yes", "No"))
                    {
                        await _repository.DeleteAll();
                        //Items.Clear();
                        //Items.TrimExcess;
                        LoadItems();
//                        ItemTotal = "0";
//                        ItemSubTotal = "0";
                    }
                });
            }
        }
        public string SubTotal()
        {
            decimal subtotal = 0M;
            foreach (Item item in Items)
            {
                subtotal += System.Convert.ToDecimal(item.Cost);
            }
            return subtotal.ToString();
        }

        public string Total(string sub)
        {
            decimal total = System.Convert.ToDecimal(sub);
            total += (total * .1M);
            return total.ToString();
        }
    }
}

                    