using System;
using FluentAssertions;
using Microsoft.Xna.Framework;
using SQJ22;
using Xunit;

namespace TestSqj;

public class GridTests
{
    [Fact]
    public void can_place_entity_on_grid()
    {
        var grid = new GridSpace();
        var entity = new EntityData(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 1),
            new EmptyRenderer()
        );

        grid.AddEntityFromData(entity, new Point(3, 4));
        grid.GetEntityAt(new Point(0, 0)).Should().BeNull();
        grid.GetEntityAt(new Point(3, 4)).Should().Be(entity);
        grid.GetEntityAt(new Point(4, 5)).Should().Be(entity);
    }

    [Fact]
    public void can_place_two_entities()
    {
        var grid = new GridSpace();
        var firstEntity = new EntityData(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(0, 1)
                .AddCell(1, 1),
            new EmptyRenderer()
        );

        var secondEntity = new EntityData(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(0, 1)
                .AddCell(1, 1),
            new EmptyRenderer()
        );

        grid.AddEntityFromData(firstEntity, new Point(0, 0));
        grid.AddEntityFromData(secondEntity, new Point(2, 0));
    }

    [Fact]
    public void cannot_add_overlapping_entities()
    {
        var grid = new GridSpace();
        var firstEntity = new EntityData(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(0, 1)
                .AddCell(1, 1),
            new EmptyRenderer()
        );

        var secondEntity = new EntityData(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(0, 1)
                .AddCell(1, 1),
            new EmptyRenderer()
        );

        var func = () =>
        {
            grid.AddEntityFromData(firstEntity, new Point(0, 0));
            grid.AddEntityFromData(secondEntity, new Point(1, 1));
        };
        func.Should().Throw<Exception>();
        
        grid.CanAddEntity(secondEntity, new Point(1, 1)).Should().BeFalse();
    }

    [Fact]
    public void cannot_add_same_entity_twice()
    {
        var grid = new GridSpace();
        var entity = new EntityData(
            new Grid()
                .AddCell(0, 0)
                .AddCell(1, 0)
                .AddCell(0, 1)
                .AddCell(1, 1),
            new EmptyRenderer()
        );

        var func = () =>
        {
            grid.AddEntityFromData(entity, new Point(0, 0));
            grid.AddEntityFromData(entity, new Point(5, 5));
        };
        func.Should().Throw<Exception>();

        grid.CanAddEntity(entity, new Point(5, 5)).Should().BeFalse();
    }
}
