using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace PlaceClient
{
    class PlaceBUS
    {
        static IFirebaseConfig config = new FirebaseConfig { BasePath = "https://lab-sa-2231-default-rtdb.asia-southeast1.firebasedatabase.app/" };
        static FirebaseClient client = new FirebaseClient(config);

        public async void ListenFirebase(DataGridView dgvPlace)
        {
            await client.OnAsync("places",
                added: (sender, args, context) => { UpdateDataGridView(dgvPlace); },
                changed: (sender, args, context) => { UpdateDataGridView(dgvPlace); },
                removed: (sender, args, context) => { UpdateDataGridView(dgvPlace); }
            );
        }

        private void UpdateDataGridView(DataGridView dgvPlace)
        {
            dgvPlace.BeginInvoke(new MethodInvoker(delegate {
                List<Place> places = GetAll();
                dgvPlace.DataSource = places;
            })); // set asynchronous datasource 
        }

        private List<Place> GetAll()
        {
            FirebaseResponse response = client.Get("places");
            Dictionary<String, Place> dictPlaces = response.ResultAs<Dictionary<String, Place>>();
            return dictPlaces.Values.ToList();
        }

        public Place GetDetails(int code)
        {
            FirebaseResponse response = client.Get("places/P" + code);
            Place place = response.ResultAs<Place>();
            return place;
        }

        private String GetKeyByCode(int code)
        {
            FirebaseResponse response = client.Get("places");
            Dictionary<String, Place> dictPlaces = response.ResultAs<Dictionary<String, Place>>();
            String key = dictPlaces.FirstOrDefault(x => x.Value.Code == code).Key;
            return key;
        }

        public List<Place> Search(String keyworđ)
        {
            List<Place> places = new List<Place>();
            foreach (var item in GetAll())
            {
                if (item.Name.ToLower().Contains(keyworđ.ToLower()))
                {
                    places.Add(item);
                }
            }
            return places;
        }

        public bool AddNew(Place newPlace)
        {
            try
            {
                client.Set("places/P" + newPlace.Code, newPlace);
                return true;
            }
            catch { return false; }

        }

        public bool Update(Place newPlace)
        {
            try
            {
                String key = GetKeyByCode(newPlace.Code);
                if (String.IsNullOrEmpty(key)) return false;
                client.Set("places/" + key, newPlace);
                return true;

            }
            catch { return false; }
        }

        public bool Delete(int code)
        {
            try
            {
                String key = GetKeyByCode(code);
                if (String.IsNullOrEmpty(key)) return false;
                client.Delete("places/" + key);
                return true;
            }
            catch { return false; }
        }
    }
}
