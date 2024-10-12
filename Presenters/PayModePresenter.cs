using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket_mvp.Views;
using Supermarket_mvp.Models;
using Supermarket_mvp.Presenters.common;

    



namespace Supermarker_mvp.Presenters
{
    internal class PayModePresenter 

    {
        
        private IPayModeView view;
        private IPayModeRepository repository;
        private BindingSource payModeBindingSource;
        private IEnumerable<PayModeModel> payModeList;
        public PayModePresenter(IPayModeView view, IPayModeRepository repository)
        {
            this.payModeBindingSource = new BindingSource();
            this.view = view;
            this.repository = repository;

            this.view.SearchEvent += SearchPayMode;
            this.view.AddNewEvent += AddNewPayMode; 
            this.view.EditEvent += LoadSelectPayModeToEdit;
            this.view.DeleteEvent += DeleteSelectedPayMode;
            this.view.SaveEvent += SavePayMode;
            this.view.CancelEvent += CancelAction;

            this.view.SetPayModeListBildingSource(payModeBindingSource);

            LoadAllPayModeList();

            this.view.Show();
        }
            
        private void LoadAllPayModeList()
        {
            payModeList = repository.GetAll();
            payModeBindingSource.DataSource = payModeList;
        }

        private void CancelAction(object? sender, EventArgs e)
        {
            CleanViewFields();  
        }

        private void SavePayMode(object? sender, EventArgs e)
        {
            
            var payMode = new PayModeModel();
            payMode.Id = Convert.ToInt32(view.PayModeId);
            payMode.Name = view.PayModeName;
            payMode.Observation = view.PayModeObservation;

            try
            {
               
                new Supermarket_mvp.Presenters.common.ModelDataValidation().Validate(payMode);

                if (view.IsEdit)

                {
                    repository.Edit(payMode);
                    view.Message = "PayMode edited successfully";
                }
                else
                {
                    repository.Add(payMode);
                    view.Message = "PayMode added successfully";
                }
                view.IsSuccessful = true;
                LoadAllPayModeList();
                CleanViewFields();
            }
            catch (Exception ex)
            {
                // Si ocurre una excepcion se configura IsSuccesfull en false
                // y a la propiedad Message de la vista se asigna el mensaje de la exception
                view.IsSuccessful = false;
                view.Message = ex.Message;
            }
        }

        private void CleanViewFields()
        {
            view.PayModeId = "0";
            view.PayModeName = "";
            view.PayModeObservation = "";
        }
            
        private void DeleteSelectedPayMode(object? sender, EventArgs e)
        {
            try
            {
                // Se recupera el objeto de la fila seleccionada del dataviewgrid
                var payNode = (PayModeModel?)payModeBindingSource.Current;

                // Se invoca el método Delete del repositorio pasándole el ID del Pay Node
                repository.Delete(payNode.Id);
                view.IsSuccessful = true;
                view.Message = "Pay Node deleted successfully";
                LoadAllPayModeList();
            }
            catch (Exception ex)
            {
                view.IsSuccessful = false;
                view.Message = "An error occurred, could not delete pay node";
            }
        }

        private void LoadSelectPayModeToEdit(object? sender, EventArgs e)
        {
            // Se obtiene el objeto del datagridview que se encuentra seleccionado
            var payMode = (PayModeModel)payModeBindingSource.Current;

            // Se cambia el contenido de las cajas de texto por el objeto recuperado
            // del datagridview
            view.PayModeId = payMode.Id.ToString();
            view.PayModeName = payMode.Name;
            view.PayModeObservation = payMode.Observation;

            // Se establece el modo como edición
            view.IsEdit = true;
        }

        private void AddNewPayMode(object? sender, EventArgs e)
        {
            view.IsEdit = false;
        }

        private void SearchPayMode(object? sender, EventArgs e)
        {
            bool emptyValue = string.IsNullOrWhiteSpace(this.view.SearchValue);
            if (emptyValue == false)
            {
                payModeList = repository.GetByValue(this.view.SearchValue);
            }
            else
            {           
                payModeList = repository.GetAll();
            }
            payModeBindingSource.DataSource = payModeList;
        }
    }
}




    