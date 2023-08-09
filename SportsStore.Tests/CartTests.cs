using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests {
    public class CartTests {
        [Fact]
        public void CanAddNewLines() {
            // Arrange
            Cart cart = new Cart();
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };

            // Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            CartLine[] results = cart.Lines.ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal("P1", results[0].Product.Name);
            Assert.Equal(1, results[0].Quantity);
            Assert.Equal("P2", results[1].Product.Name);
            Assert.Equal(3, results[1].Quantity);
        }
        [Fact]
        public void CanAddQuantityForExistingProduct() {
            // Arrange
            Cart cart = new Cart();
            Product p1 = new Product() { ProductID = 1, Name = "P1" };

            // Act
            cart.AddItem(p1, 1);
            cart.AddItem(p1, 2);
            CartLine[] results = cart.Lines.ToArray();

            // Assert
            Assert.Single(results);
            Assert.Equal(3, results[0].Quantity);
        }
        [Fact]
        public void CanRemoveLine() {
            // Arrange
            Cart cart = new Cart();
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };
            Product p3 = new Product() { ProductID = 3, Name = "P3" };

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 1);

            // Act
            cart.RemoveLine(p2);
            CartLine[] results = cart.Lines.ToArray();

            // Assert
            Assert.Equal(2, results.Length);
            Assert.Empty(results.Where(l => l.Product == p2));
        }
        [Fact]
        public void CanCalculateTotalValue() {
            // Arrange
            Cart cart = new Cart();
            Product p1 = new Product() { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product() { ProductID = 2, Name = "P2", Price = 50M };

            // Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);
            decimal result = cart.ComputeTotalValue();

            // Assert
            Assert.Equal(450M, result);
        }
        [Fact]
        public void CanClearCart() {
            // Arrange
            Cart cart = new Cart();
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);

            // Act
            cart.Clear();

            // Assert
            Assert.Empty(cart.Lines);
        }
    }
}
