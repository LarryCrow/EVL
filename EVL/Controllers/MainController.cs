﻿using EVL.Model;
using EVL.Views;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Controllers
{
    public class MainController
    {
        private readonly ViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public MainController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }

        public DataImportView CreateDataImportView()
        {
            return new DataImportView(viewState, new ImportController(viewState, createDbContext));
        }

        public DataBaseView CreateDataBaseView(int projectId)
        {
            return new DataBaseView(new ReadOnlyDatabaseViewState(projectId, createDbContext()));
        }

        public NewDataView CreateNewDataView(int projectId)
        {
            var ndvs = new NewDataViewState(projectId);
            return new NewDataView(ndvs, new NewDataController(ndvs, createDbContext));
        }

        public FactorsView CreateFactorsView(int projectId)
        {
            var fvs = new FactorsViewState(projectId, createDbContext());
            return new FactorsView(fvs, new FactorsController(fvs, createDbContext));
        }

        public ProjectsView CreateProjectsView()
        {
            return new ProjectsView(viewState, new ProjectController(viewState, createDbContext));
        }
    }
}