using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Assignment.Models;

namespace Assignment.Controllers
{
    public class PlantsController : ApiController
    {
        private PlantDb db = new PlantDb();

        // GET: api/Plants
        public IQueryable<Plant> GetPlants()
        {
            return db.Plants;
        }

        // GET: api/Plants/5
        [ResponseType(typeof(Plant))]
        public IHttpActionResult GetPlant(int id)
        {
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return NotFound();
            }

            return Ok(plant);
        }

        //// PUT: api/Plants/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutPlant(int id, Plant plant)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != plant.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(plant).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PlantExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}



        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [Route("api/Plants/{id}/watered")]
        public IQueryable<Plant> FinishWateringPlant(int id, Plant plant)
        {
            DbSet<Plant> pl = (DbSet<Plant>)GetPlants();

            plant = db.Plants.Find(id);
            // plant = pl.Find(x => x.Id == id);


            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return GetPlants();

            }

            if (id != plant.Id)
            {
                // return BadRequest();
                return GetPlants();

            }

            db.Entry(plant).State = EntityState.Modified;

            plant.Watered_Status = 1; //plant is now watered

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    //  return NotFound();
                    return GetPlants();

                }
                else
                {
                    throw;
                }
            }

            return GetPlants();
            //return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Plants/5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        public IQueryable<Plant> BeginWaterPlant(int id, Plant plant)
        {
            DbSet<Plant> pl = (DbSet<Plant>)GetPlants();

            plant = db.Plants.Find(id);
            // plant = pl.Find(x => x.Id == id);


            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return GetPlants();

            }

            if (id != plant.Id)
            {
                // return BadRequest();
                return GetPlants();

            }

            db.Entry(plant).State = EntityState.Modified;

            plant.Watered_Status = 2; //plant is now being watered
            plant.Last_Watered = DateTime.Now;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    //  return NotFound();
                    return GetPlants();

                }
                else
                {
                    throw;
                }
            }

            return GetPlants();
            //return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Plants/5
        [ResponseType(typeof(void))]
        [AcceptVerbs("PUT")]
        [Route("api/Plants/{id}/revert")]

        public IQueryable<Plant> RevertWaterDBCall(int id, Plant plant)
        {
            DbSet<Plant> pl = (DbSet<Plant>)GetPlants();

            plant = db.Plants.Find(id);
            // plant = pl.Find(x => x.Id == id);


            if (!ModelState.IsValid)
            {
                // return BadRequest(ModelState);
                return GetPlants();

            }

            if (id != plant.Id)
            {
                // return BadRequest();
                return GetPlants();

            }

            db.Entry(plant).State = EntityState.Modified;

            plant.Watered_Status = 1; //plant is now being watered

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    //  return NotFound();
                    return GetPlants();

                }
                else
                {
                    throw;
                }
            }

            return GetPlants();
            //return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Plants
        [ResponseType(typeof(Plant))]
        public IHttpActionResult PostPlant(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Plants.Add(plant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = plant.Id }, plant);
        }

        // DELETE: api/Plants/5
        [ResponseType(typeof(Plant))]
        public IHttpActionResult DeletePlant(int id)
        {
            Plant plant = db.Plants.Find(id);
            if (plant == null)
            {
                return NotFound();
            }

            db.Plants.Remove(plant);
            db.SaveChanges();

            return Ok(plant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlantExists(int id)
        {
            return db.Plants.Count(e => e.Id == id) > 0;
        }
    }
}