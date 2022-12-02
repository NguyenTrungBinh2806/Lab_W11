using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaceClient
{
    public partial class PlaceForm : Form
    {
        public PlaceForm()
        {
            InitializeComponent();
        }

        private void PlaceForm_Load(object sender, EventArgs e)
        {
            new PlaceBUS().ListenFirebase(dgvPlace);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            String keyword = txtKeyword.Text.Trim();
            List<Place> places = new PlaceBUS().Search(keyword);
            dgvPlace.BeginInvoke(new MethodInvoker(delegate { dgvPlace.DataSource = places; }));
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dgvPlace_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvPlace.SelectedRows.Count > 0)
            {
                int code = int.Parse(dgvPlace.SelectedRows[0].Cells["Code"].Value.ToString());
                Place place = new PlaceBUS().GetDetails(code);
                if(place != null)
                {
                    txtCode.Text = place.Code.ToString();
                    txtName.Text = place.Name;
                    txtAddress.Text = place.Address;
                    txtDescription.Text = place.Description;
                    txtNation.Text = place.Nation;
                    txtRate.Text = place.Rate.ToString();
                    txtImage.Text = place.Image;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Place newPlace = new Place()
            {
                Code = int.Parse(txtCode.Text.Trim()),
                Name = txtName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Nation = txtNation.Text.Trim(),
                Rate = int.Parse(txtRate.Text.Trim()),
                Image = txtImage.Text.Trim(),
            };
            bool result = new PlaceBUS().AddNew(newPlace);
            if (!result)
            {
                MessageBox.Show("Sorry! can not add this place");
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Place newPlace = new Place()
            {
                Code = int.Parse(txtCode.Text.Trim()),
                Name = txtName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Nation = txtNation.Text.Trim(),
                Rate = int.Parse(txtRate.Text.Trim()),
                Image = txtImage.Text.Trim()
            };
            bool result = new PlaceBUS().Update(newPlace);
            if (!result)
            {
                MessageBox.Show("Sorry! can not update this place");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "CONFIRMATION", MessageBoxButtons.YesNo);
            if(dialogResult == DialogResult.Yes)
            {
                int code = int.Parse(txtCode.Text);
                bool result = new PlaceBUS().Delete(code);
                if (!result)
                {
                    MessageBox.Show("Sorry! can not delete this place");
                }
            }
        }

       
    }
}
