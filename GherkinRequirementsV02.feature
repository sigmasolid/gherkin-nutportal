Feature: Find the best nuts in the forest
As a hungry squirrel
I want to find the best and ripest nuts in the forest
So that I can optimize my nut-gathering efficiency

    Scenario: 01 - If I request information about a tree the correct tree is returned
        Given the forest has an "oak" tree with the name "oak-1" and 22 "ripe" nuts
        When I request information about the tree with the name "oak-1"
        Then the response should include the tree named "oak-1" with 22 "ripe" nuts

    Scenario: 02 - Retrieve the tree with the most ripe chestnuts
        Given the forest has the following trees:
          | TreeName   | TreeType | Ripeness | NumberOfNuts |
          | hazelnut-1 | hazelnut | ripe     | 12           |
          | oak-1      | oak      | ripe     | 8            |
          | chestnut-1 | chestnut | ripe     | 25           |
          | hazelnut-2 | hazelnut | green    | 30           |
        When I query the API for the tree with the most ripe nuts of type "chestnut"
        Then the response should return the tree "chestnut-1"
        And the nut count should be 25